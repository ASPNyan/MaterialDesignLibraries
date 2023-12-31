export function SwapElementColors(elementId) {
    let element = document.getElementById(elementId);
    let color = element.style.color;
    element.style.color = element.style.backgroundColor;
    element.style.backgroundColor = color;
}