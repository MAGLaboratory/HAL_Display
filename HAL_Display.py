# This Python file uses the following encoding: utf-8
import sys
import signal
import os
from dataclasses import dataclass
from typing import *

from PySide2.QtGui import QGuiApplication
from PySide2.QtQml import QQmlApplicationEngine
from PySide2.QtCore import QCoreApplication, QObject, Slot
from PySide2 import QtCore

def sigint_handler(*args):
    print("signal handler");
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
    print("%s: %s (%s:%d, %s)" % (mode, message, context.file, context.line, context.file))

if __name__ == "__main__":
    signal.signal(signal.SIGINT, sigint_handler)
    QtCore.qInstallMessageHandler(qt_message_handler)

    app = QGuiApplication(sys.argv)
    engine = QQmlApplicationEngine()
    engine.load(os.path.join(os.path.dirname(__file__), "HAL_Display.qml"))

    if not engine.rootObjects():
        sys.exit(-1)

    win = engine.rootObjects()[0]
    # workaround in windows to make the application not always be on top
    win.setProperty("visibility", "FullScreen")

    sys.exit(app.exec_())
