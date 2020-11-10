import QtQuick 2.11
import QtQuick.Controls 2.4

ApplicationWindow {
    id: window
    objectName: "MainWindow"
    width: 600
    height: 1024
    visible: true
    visibility: "Minimized"
    title: qsTr("HAL Display")

    Component.onCompleted:
    {
        console.debug("QML Loaded")
    }

    Item {
        id: tFullScreen_Handler
        focus: true
        Keys.onPressed: {
            if (event.key === Qt.Key_F11)
            {
                if (window.visibility === ApplicationWindow.Windowed)
                    window.visibility = ApplicationWindow.FullScreen
                else
                    window.visibility = ApplicationWindow.Windowed
            }
            if (event.key === Qt.Key_Escape)
            {
                Qt.quit()
            }
        }
    }

    function thermo (ctx, x, y, temp, nok)
    {
        ctx.font = "20px Arial"
        // ok font color handled by caller
        if (nok !== 0)
        {
            ctx.strokeStyle = Qt.rgba(1,0,0,1)
            ctx.fillStyle = Qt.rgba(1,0,0,1)
            temp = "XX"
        }
        ctx.fillText("🌡️" + temp.toString().padStart(2, '0') + "°C", x + 3.5, y + 24)
        ctx.strokeRect(x, y, 72, 35)

        if (nok !== 0)
        {
            ctx.beginPath()
            ctx.path = "M"+x+","+y+" l 72,35"
            ctx.stroke()
            ctx.beginPath()
            ctx.path = "M"+x+","+(y + 35)+" l 72,-35"
            ctx.stroke()
        }
    }

    function motions(ctx, x, y, motion, nok)
    {
        if (nok !== 0)
        {
            ctx.strokeStyle = Qt.rgba(1,0,0,1)
        }
        ctx.beginPath()
        ctx.arc(x + 65, y, 6, 0, Math.PI)
        ctx.fill()
        ctx.lineWidth = 1
        ctx.beginPath()
        ctx.arc(x + 65, y, 20, Math.PI/2, Math.PI)
        ctx.stroke()
        ctx.beginPath()
        ctx.arc(x + 65, y, 40, Math.PI/2, Math.PI)
        ctx.stroke()
        if (true)
        {
            var st = ctx.strokeStyle
            var fl = ctx.fillStyle
            var lw = ctx.lineWidth
            ctx.strokeStyle = Qt.rgba(0.2,1,0.2,1)
            ctx.fillStyle = Qt.rgba(0.2,1,0.2,1)
            ctx.beginPath()
            ctx.arc(x + 41.5, y + 21, 7, 0, 2*Math.PI)
            ctx.fill()
            ctx.lineWidth = 12
            ctx.lineCap = "round"
            ctx.beginPath()
            ctx.path = "M" + (x+38) + "," + (y+36) + " l  -4,21"
            ctx.stroke()
            ctx.lineWidth = 4
            ctx.beginPath()
            ctx.path = "M" + (x+38) + "," + (y+32.5) + "l -10,4 l -4.5,11"
            ctx.stroke()
            ctx.beginPath()
            ctx.path = "M" + (x+39) + "," + (y+32) + "l 9,13 l 9,2"
            ctx.stroke()
            ctx.lineWidth = 5
            ctx.beginPath()
            ctx.path = "M" + (x+30) + "," + (y+60) + "l -3,10 l -7,10"
            ctx.stroke()
            ctx.beginPath()
            ctx.path = "M" + (x+37) + "," + (y+60) + "l 4,10, l 0,10"
            ctx.stroke()
            ctx.lineCap = "butt"
            ctx.strokeStyle = st
            ctx.fillStyle = fl
            ctx.lineWidth = lw
        }
        ctx.lineWidth = 2
        ctx.strokeRect(x, y, 72, 88)
    }

    Canvas {
        id: synoptic
        objectName: "synoptic"
        width: 600
        height: 400
        property int front_door: 0
        property int pod_bay_door: 0
        property int office_motion
        property int shop_motion
        property int confRm_motion
        property int elecRm_motion
        property int shopB_motion
        property int outdoor_temp
        property int bay_temp
        property int confRm_temp
        property int elecRm_temp
        property int shopB_temp
        property int daisy_techNOk
        property int hal_techNOk
        property string space_open: "Closed"
        onPaint: {
            var ctx = getContext("2d")
            ctx.fillStyle = Qt.rgba(1, 1, 0.9, 1)
            ctx.fillRect(0, 0, width, height)
            ctx.fillStyle = Qt.rgba(0.4, 0.4, 0.4, 1)
            ctx.fillRect(0, 0, 90, height)
            ctx.lineWidth = 4
            ctx.strokeStyle = Qt.rgba(0, 0, 0, 1)
            // outer wall
            // ctx.strokeRect(92, 12, 496, 376)
            ctx.beginPath()
            ctx.path = "M92,35 v -23 h 496 v 376 h -496 v -25"
            ctx.stroke()
            ctx.beginPath()
            ctx.path = "M92,209 V299"
            ctx.stroke()
            // kitchen
            ctx.beginPath()
            ctx.path = "M92,230 h 96"
            ctx.stroke()
            ctx.beginPath()
            ctx.path = "M238,230 h 14 v 158"
            ctx.stroke()
            // bathroom
            //ctx.strokeRect(252, 290, 80, 98)
            ctx.beginPath()
            ctx.path = "M252,290 h 15"
            ctx.stroke()
            ctx.beginPath()
            ctx.path = "M317,290 h 15 v 98"
            ctx.stroke()
            // conference room
            //ctx.strokeRect(360, 12, 228, 140)
            ctx.beginPath()
            ctx.path = "M360,12 v 140 h 40"
            ctx.stroke()
            ctx.beginPath()
            ctx.path = "M450,152 h 138"
            ctx.stroke()
            // electronics room
            //ctx.strokeRect(460, 152, 128, 236)
            ctx.beginPath()
            ctx.path = "M460,152 v 40"
            ctx.stroke()
            ctx.beginPath()
            ctx.path = "M460,242 V 388"
            ctx.stroke()

            // setup for doors
            ctx.lineWidth = 2
            // unmonitored doors
            ctx.strokeStyle = Qt.rgba(0.4, 0.4, 0.4, 1)
              // kitchen door
            ctx.beginPath()
            ctx.path = "M188,230 h 50"
            ctx.stroke()
              // bathroom door
            ctx.beginPath()
            ctx.path = "M267,290 H 317"
            ctx.stroke()
              // confRm door
            ctx.beginPath()
            ctx.path = "M400,152 h 50"
            ctx.stroke()
              // elecRm door
            ctx.beginPath()
            ctx.path = "M460,192 v 50"
            ctx.stroke()
            // bay door
            if (pod_bay_door == 1)
            {
                // stroke top of the open bay door
                ctx.strokeStyle = Qt.rgba(0.9, 0.8, 0, 1)
                ctx.beginPath()
                ctx.path = "M92,35 v 20"
                ctx.stroke()
                // stroke bottom of the open bay door
                ctx.beginPath()
                ctx.path = "M92,209 v -20"
                ctx.stroke()
            }
            else
            {
                ctx.strokeStyle = Qt.rgba(0.2, 0.8, 0.2, 1)
                ctx.lineWidth = 2
                ctx.beginPath()
                ctx.path = "M92,35 V 209"
                ctx.stroke()
            }
            // front door
            if (front_door == 1)
            {
                // front door open
                ctx.strokeStyle = Qt.rgba(0.9, 0.8, 0, 1)
                ctx.lineCap = "round"
                ctx.beginPath()
                ctx.path = "M92,362 l -58,-26"
                ctx.stroke()
                ctx.lineCap = "butt"
            }
            else
            {
                // front door closed
                ctx.strokeStyle = Qt.rgba(0.2, 0.8, 0.2, 1)
                ctx.beginPath()
                ctx.path = "M92,363 v -64"
                ctx.stroke()
            }
            // thermostats
            ctx.fillStyle = Qt.rgba(0,0,0,1)
            ctx.strokeStyle = Qt.rgba(0,0,0,1)
            thermo(ctx, 112, 151, bay_temp, hal_techNOk)
            ctx.fillStyle = Qt.rgba(0,0,0,1)
            ctx.strokeStyle = Qt.rgba(0,0,0,1)
            thermo(ctx, 383, 249, shopB_temp, daisy_techNOk)
            thermo(ctx, 465, 196, elecRm_temp, daisy_techNOk)
            thermo(ctx, 386, 112, confRm_temp, daisy_techNOk)
            // special outdoor thermometer
            ctx.fillStyle = Qt.rgba(1,1,1,1)
            ctx.strokeStyle = Qt.rgba(1,1,1,1)
            thermo(ctx, 15, 16, outdoor_temp, hal_techNOk)
            // motion sensors
            ctx.fillStyle = Qt.rgba(0,0,0,1)
            ctx.strokeStyle = Qt.rgba(0,0,0,1)
            motions(ctx, 188, 137, shop_motion, hal_techNOk)
            motions(ctx, 97, 246, office_motion, hal_techNOk)
            motions(ctx, 383, 157, shopB_motion, daisy_techNOk)
            motions(ctx, 465, 235, elecRm_motion, daisy_techNOk)
            motions(ctx, 462, 59, confRm_motion, daisy_techNOk)
            // systems
            ctx.lineWidth = 2
              // HAL
            ctx.fillStyle = Qt.rgba(0,0,0,1)
            ctx.strokeStyle = Qt.rgba(0,0,0,1)
            ctx.font = "30px Arial"
            ctx.fillText("HAL", 119, 218)
            ctx.strokeRect(112, 190, 72, 35)
              // Daisy
            ctx.font = "26px Arial"
            ctx.fillText("Daisy", 468, 183)
            ctx.strokeRect(465, 157, 72, 35)
            // space open
            ctx.font = "36px Arial"
            ctx.fillText("Space", 110, 50)
            ctx.fillText(space_open, 110, 90)
        }
    }
}
