# This Python file uses the following encoding: utf-8
import sys, signal, os, json, time, traceback, json
import paho.mqtt.client as mqtt
from dataclasses import dataclass
from dataclasses_json import dataclass_json
from typing import *

from PySide2.QtGui import QGuiApplication
from PySide2.QtQml import QQmlApplicationEngine
from PySide2.QtCore import QCoreApplication, QObject, Slot, QTimer, QAbstractItemModel
from PySide2 import QtCore
from PySide2.QtWidgets import QTreeView
from PySide2.QtQuickControls2 import QQuickStyle


def sigint_handler(*args):
    print("signal handler")
    # Handler for SIGINT signal
    QGuiApplication.quit()


def qt_message_handler(mode, context, message):
    if mode == QtCore.QtInfoMsg:
        mode = 'Info'
    elif mode == QtCore.QtWarningMsg:
        mode = 'Warning'
    elif mode == QtCore.QtCriticalMsg:
        mode = 'critical'
    elif mode == QtCore.QtFatalMsg:
        mode = 'fatal'
    else:
        mode = 'Debug'
    print("%s: %s (%s:%d, %s)" % (mode, message,
                                  context.file, context.line, context.file))


class HAL_Display(mqtt.Client):
    version = "2020"
    # Do it this order or bad things will happen.  Guaranteed.
    @dataclass_json
    @dataclass
    class config:
        name: str               # really only here because it looks better
        description: str        # see above
        checkup_freq: int       # we expect the HAL Reporter to send checkups
        checkup_buffer: int     # how many checkups to miss before bad
        daemons: List[str]      # names of the daemons we should look for
        subtopics: List[str]
        mqtt_broker: str
        mqtt_port: int
        mqtt_timeout: int

    @dataclass
    class data:
        sensors: List[Dict[str, str]]
        messages: Dict[str, str]

    # Use Qt, they said.  It would be easy, they said!
    class sensorNode(QObject):
        def __init__(self, name="", parent=None, message=""):
            self._parent = parent
            self._name = name
            self._message = message
            self._children = []

        def children(self):
            return self._children

        def hasChildren(self):
            return bool(self.children())

        def parent(self):
            return self._parent

        def name(self):
            return self._name

        def set_name(self, name):
            self._name = name

        def message(self):
            return self._message

        def set_message(self, message):
            self._message = message

        def columnCount(self):
            return 2

        def child_count(self):
            return len(self._children)

        def add_child(self, child):
            self._children.append(child)
            child._parent = self

        def insert_child(self, position, child):
            if 0 <= position < child_count:
                self._children.insert(position, child)
                child._parent = self
                return True
            return False

        # don't need a child remove function
        def row(self):
            if self._parent is not None:
                return self._parent._children.index(self)
            return -1

        def child(self, row):
            if 0 <= row < self.child_count():
                return self._children[row]

        def find_child_by_name(self, name):
            for child in self._children:
                if child.name() == name:
                    return child
            return None

        # nobody is perfect
        def debug_out(self, tab_level=-1):
            output = ""
            tab_level += 1

            for i in range(tab_level):
                output += "\t"

            output += "|____" + self._name + " " + self._message + "\n"

            for child in self._children:
                output += child.debug_out(tab_level)

            tab_level -= 1

            return output

        def __repr__(self):
            return self.debug_out()

    class sensorTable(QAbstractItemModel):
        def __init__(self, parent=None):
            super().__init__(parent)
            self._root_node = HAL_Display.sensorNode()

        def index(self, row, column, parent):
            if not self.hasIndex(row, column, parent):
                return QtCore.QModelIndex()
            node = parent.internalPointer() if parent.isValid() else self._root_node
            if node.children:
                return self.createIndex(row, column, node.child(row))
            else:
                return QtCore.QModelIndex()

        def parent(self, child):
            if not child.isValid():
                return QtCore.QModelIndex()
            node = child.internalPointer()
            if node.row() >= 0:
                return self.createIndex(node.row(), 0, node.parent())
            return QtCore.QModelIndex()

        def rowCount(self, parent=QtCore.QModelIndex()):
            node = parent.internalPointer() if parent.isValid() else self._root_node
            return node.child_count()

        def columnCount(self, parent=QtCore.QModelIndex()):
            return 1

        def hasChildren(self, parent=QtCore.QModelIndex()):
            node = parent.internalPointer() if parent.isValid() else self._root_node
            return node.hasChildren()

        def data(self, index: QtCore.QModelIndex, role=QtCore.Qt.DisplayRole):
            if index.isValid():
                print (role)
                if role in (QtCore.Qt.DisplayRole, QtCore.Qt.EditRole):
                    node = index.internalPointer()
                    return node.name()
                if role == (QtCore.Qt.UserRole + 3):
                    node = index.internalPointer()
                    return node.message()

        def setData(self, index, value, role=QtCore.Qt.EditRole):
            if role == (QtCore.Qt.UserRole+3):
                node = index.internalPointer()
                node.set_message(value)
                self.dataChanged.emit(index, index)
                return True
            return False

        def flags(self, index: QtCore.QModelIndex):
            return QtCore.Qt.ItemIsEnabled | QtCore.Qt.ItemIsSelectable

        def indexFromItem(self, it):
            root_index = QtCore.QModelIndex()
            if isinstance(it, HAL_Display.sensorNode):
                parents = []
                while it is not self._root_node:
                    parents.append(it)
                    it = it.parent()
                root = self._root_node
                for parent in reversed(parents):
                    root = root.find_child_by_name(parent.name())
                    root_index = self.index(root.row(), 0, root_index)
            return root_index

        def item_from_path(self, path, sep):
            depth = path.split(sep)
            root = self._root_node
            for d in depth:
                root = root.find_child_by_name(d)
                if root is None:
                    return None
            return root

        def insertRows(self, position, items, parent=None):
            parent_index = self.indexFromItem(parent)
            self.beginInsertRows(parent_index, position, position + len(items) - 1)
            if parent is None:
                parent = self._root_node
            for item in items:
                parent.add_child(item)
            self.endInsertRows()

        def updateSensor(self, sensor, value, sep):
            depth = sensor.split(sep)
            it = self._root_node
            last = self._root_node
            for d in depth:
                last = it
                it = it.find_child_by_name(d)
                if it is None:
                    index = self.indexFromItem(last)
                    self.insertRows(index.row(), [HAL_Display.sensorNode(d)], last)
                    it = last.find_child_by_name(d)
            self.setData(self.indexFromItem(it), value, QtCore.Qt.UserRole+3)

        def roleNames(self):
            retval = {QtCore.Qt.DisplayRole: QtCore.QByteArray(b'name'),
                QtCore.Qt.ItemDataRole(QtCore.Qt.UserRole+3): QtCore.QByteArray(b'data')}
            print(retval)
            return retval

    # Overloaded MQTT functions (from mqtt.Client)
    def on_log(self, client, userdata, level, buff):
        if level != mqtt.MQTT_LOG_DEBUG:
            print(level)
            print(buff)
        if level == mqtt.MQTT_LOG_ERR:
            print("critical error encountered")
            traceback.print_exc()
            self.running = False
            self.app.exit()

    def on_connect(self, client, userdata, flags, rc):
        print("Connected: " + str(rc))
        self.subscribe("reporter/checkup_req")
        self.subscribe("reporter/bootup")
        for daemon in self.config.daemons:
            for subtopic in self.config.subtopics:
                print("Subscribing to: " + daemon + "/" + subtopic)
                self.subscribe(daemon + "/" + subtopic)

    def on_message(self, client, userdata, message):
        if not message.payload:
            print("Topic received: " + message.topic)
            return
        pl = message.payload.decode("utf-8")
        print("Topic received: " + message.topic + " :: " + pl)
        self.data.messages[message.topic] = pl
        path = message.topic.split('/')
        newsensors = {}
        sensors_json = json.loads(pl)
        for sensor, value in sensors_json.items():
            self.sensorTableModel.updateSensor(path[0]+'/'+sensor, str(value), '/')


    # Run function, handles everything
    def run(self):
        signal.signal(signal.SIGINT, sigint_handler)
        QtCore.qInstallMessageHandler(qt_message_handler)

        my_path = os.path.dirname(os.path.abspath(__file__))
        conf_file = open(my_path + "/HAL_Display.json", "r")
        self.config = HAL_Display.config.from_json(conf_file.read())
        conf_file.close()

        self.data.messages = {}
        self.data.sensors = {}

        print("Config loaded:")
        print(self.config)

        if self.config.checkup_freq < 10:
            print("Checkup period too low")
            sys.exit(1)

        print("Connecting to MQTT server.")
        self.connect(self.config.mqtt_broker, self.config.mqtt_port, self.config.mqtt_timeout)
        self.loop_start()

        self.app = QGuiApplication([])
        QQuickStyle.setStyle("Material")
        engine = QQmlApplicationEngine()
        engine.load(os.path.join(os.path.dirname(__file__), "HAL_Display.qml"))

        if not engine.rootObjects():
            sys.exit(-1)

        win = engine.rootObjects()[0]
        # workaround in windows to make the application not always be on top
        win.setProperty("visibility", "FullScreen")
        self.synoptic = win.findChild(QObject, "synoptic")
        # synoptic.setProperty("front_door", 1)
        self.sensorTableModel = HAL_Display.sensorTable()
        self.sensorTableModel.insertRows(0,[HAL_Display.sensorNode(daemon) for daemon in self.config.daemons])
        print(self.sensorTableModel._root_node)
        engine.rootContext().setContextProperty("sensorTableModel", self.sensorTableModel)


        def openDoor():
            self.synoptic.setProperty("front_door", 1)
            self.synoptic.setProperty("space_open", "Open")
            self.synoptic.requestPaint()

            print(self.sensorTableModel.roleNames())
            print(QtCore.Qt.UserRole + 0)
        QTimer.singleShot(5000, openDoor)

        exit_code = self.app.exec_()
        self.loop_stop()
        sys.exit(exit_code)


if __name__ == "__main__":
    HD = HAL_Display()
    HD.run()
