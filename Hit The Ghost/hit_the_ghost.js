let state = "waiting"
let connected = 0
let commandVal = ""
let sepIndex = 0
let commandKey = ""
let command = ""
let icon = false
let score = 0;
let missHit = 0;
let power = 0;
let baseStrength = 0
bluetooth.setTransmitPower(7)
bluetooth.startUartService()
bluetooth.onBluetoothConnected(function () {
    connected = 1
    state = "connected"
})
bluetooth.onBluetoothDisconnected(function () {
    connected = 0
    initGame()
})
bluetooth.onUartDataReceived(serial.delimiters(Delimiters.NewLine), function () {
    command = bluetooth.uartReadUntil(serial.delimiters(Delimiters.NewLine))
    for (let j = 0; j <= command.length - 1; j++) {
        if (command.charAt(j) == "=") {
            sepIndex = j
            break;
        }
    }
    commandKey = command.substr(0, sepIndex)
    commandVal = command.substr(sepIndex + 1, command.length - sepIndex - 2)
    if (commandKey == "start") {
        baseStrength = input.acceleration(Dimension.Strength)
        state = "start"
    } else if (commandKey == "end") {
        state = "waiting"
        bluetooth.uartWriteString(control.deviceName() + "=p=" + power);
        // bluetooth.uartWriteString(control.deviceName() + "s" + score + "m" + missHit + "p" + power);
        initGame()
    } else if (commandKey == "icon") {
        icon = true
    } else if (commandKey == "noIcon") {
        icon = false
    }
})
basic.forever(function () {
    if (state == "waiting") {
        basic.showLeds(`
            . . . . .
            . . . . .
            . . . . .
            . . . . .
            # . # . #
            `)
    } else if (state == "start") {
        let s = input.acceleration(Dimension.Strength)
        // let x = input.acceleration(Dimension.X)
        // let y = input.acceleration(Dimension.Y)
        if (Math.abs(s - baseStrength) > power) {
            power = Math.abs(s - baseStrength)
        }

        if (icon) {
            basic.showIcon(IconNames.Ghost)
            if (Math.abs(s - baseStrength) > 20) {
                score += 1
                // bluetooth.uartWriteValue("score", 1)
                icon = false
                bluetooth.uartWriteString(control.deviceName() + "=s=1");
            }
        } else {
            basic.showIcon(IconNames.No)
            // basic.clearScreen();
            if (Math.abs(s - baseStrength) > 20) {
                missHit += 1
                // bluetooth.uartWriteValue("missHit", 1)
                // basic.showIcon(IconNames.Sad)
                bluetooth.uartWriteString(control.deviceName() + "=m=1");
            }
        }
    } else if (state == "connected") {
        basic.showString("C")
    }
})

function initGame() {
    score = 0;
    power = 0;
    baseStrength = 0;
    icon = false;
    state = "waiting"
}
