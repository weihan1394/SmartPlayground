let distance = 0;
let distance0 = 0;
let distance2 = 0;
let boardHit = 0;
let score = 0;
let baseTime = input.runningTime()
// let display = grove.createDisplay(DigitalPin.P2, DigitalPin.P16)
let state = 'waiting'
basic.forever(function () {

    if (state == "start") {
        distance = grove.measureInCentimeters(DigitalPin.P1)
        distance0 = grove.measureInCentimeters(DigitalPin.P0)
        distance2 = grove.measureInCentimeters(DigitalPin.P2)
        let accX = input.acceleration(Dimension.X)
        // let accY = input.acceleration(Dimension.Y)

        hitBoard(accX)

        if (distance2 < 10 || distance2 > 500 || distance0 < 10 || distance0 > 500 || distance < 10 || distance > 500) {
            serial.writeValue("c", 1)
        }
        // display.show(distance0);
    }

})

function hitBoard(accX: number) {
    if (accX >= 16) {
        boardHit++;
    }

}

function initValues() {
    boardHit = 0
}

serial.onDataReceived(serial.delimiters(Delimiters.NewLine), () => {
    let msg = serial.readLine();
    basic.showString("RX:" + msg)
    if (msg.includes('start')) {
        state = "start"
    } else if (msg.includes('end')) {
        serial.writeValue("b", boardHit)
        state = "waiting"
    }
})
