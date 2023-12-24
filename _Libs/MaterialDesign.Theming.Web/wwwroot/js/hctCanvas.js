// noinspection JSUnusedGlobalSymbols

export function renderCtCanvas(id, hexes, width, height, widthScale, heightScale, maxChroma) {
    const canvas = document.getElementById(id);
    const context = canvas.getContext("2d");

    const rectangles = [];

    for (let x = 0; x < width; x++) {
        let isMax = x >= maxChroma;
        for (let y = 0; y < height; y++) {
            // Add the rectangle to the array
            rectangles.push({
                x: x * widthScale,
                y: y * heightScale,
                width: isMax ? (width - x) * widthScale : widthScale,
                height: heightScale,
                color: hexes[y + height * x]
            });
        }
        if (isMax) break;
    }

    // Loop through the array to draw the rectangles
    for (let rectangle of rectangles) {
        context.fillStyle = rectangle.color;
        context.fillRect(rectangle.x, rectangle.y, rectangle.width, rectangle.height);
    }
    
    canvas.style.scale = "1 -1"; // flip canvas
    canvas.style.transform = `translateY(-${height})`; // reposition to correct placement
}

export function renderHCanvas(id, width, height) {
    const canvas = document.getElementById(id);
    const context = canvas.getContext("2d");
    let gradient = context.createLinearGradient(0, 0, width, height);
    
    for (let h = 0; h <= 360; h++) {
        gradient.addColorStop(h / 360, `hsl(${h}, 100%, 50%)`);
    }
    
    context.fillStyle = gradient;
    context.fillRect(0, 0, width, height);
}

export function getCanvasPos(id) {
    const canvas = document.getElementById(id).getBoundingClientRect();
    return [canvas.left, canvas.top];
}