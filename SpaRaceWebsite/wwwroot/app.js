/* 
Find the dot, buttons, and status text.

Create an empty list for location data.

When the page loads:
    Call /api/location
    Store the location points.

When Play is clicked:
    Start a timer.
    Every 200ms:
        Get the next location point.
        Move the dot to that x/y.
        Go to the next point.

When Pause is clicked:
    Stop the timer.

*/

/* *********************************************************************** */

console.log("app.js loaded");
// get the HTML elements
const dot = document.getElementById("driver-dot");
const statusText = document.getElementById("status");
const playButton = document.getElementById("play-button");
const pauseButton = document.getElementById("pause-button");
const trackArea = document.getElementById("track-area");

let locationData = []; // this list will store API data from /api/location
let currentIndex = 0; // tell us which point is currently showing -> each time the dots move -> +1 index
let intervalId = null; // stores animation timer -> if not running, NULL
let holdIntervalId = null; //For the holding on the back/forward buttons
let holdSpeed = 1; 

let minX;
let maxX;
let minY;
let maxY;

// LOAD THE API DATA USING async
async function loadLocationData() {
    statusText.textContent = "Loading location data..."; //Getting API data takes time!!

    const response = await fetch ("/api/location"); //GET DATA WAIT FOR THE DATA BEFORE PROCEEDING

    //API RESPOND COME BACK AS JSON FILE
    locationData = await response.json(); //convert json -> javascript list/object

    calculateBounds();

    statusText.textContent = `Loaded ${locationData.length} location points.`; //print how many points loaded

    console.log(locationData);
}


function calculateBounds() {
    minX = Math.min(...locationData.map(point => point.x));
    maxX = Math.max(...locationData.map(point => point.x));

    minY = Math.min(...locationData.map(point => point.y));
    maxY = Math.max(...locationData.map(point => point.y));

    console.log("Bounds:");
    console.log("minX:", minX, "maxX:", maxX);
    console.log("minY:", minY, "maxY:", maxY);
}

function scalePoint(point) {
    const trackWidth = trackArea.clientWidth;
    const trackHeight = trackArea.clientHeight;

    const padding = 40;

    const usableWidth = trackWidth - padding * 2;
    const usableHeight = trackHeight - padding * 2;

    let screenX = ((point.y - minY) / (maxY - minY)) * usableWidth + padding;
    let screenY = ((point.x - minX) / (maxX - minX)) * usableHeight + padding;

    // Keep this if left/right direction is already correct
    screenX = trackWidth - screenX;

    // Add this to flip upside down
    screenY = trackHeight - screenY;

    // Manual dot alignment controls
    const scale = 0.95;

    //Move dot left/right
    const offsetX = 9;

    //Move it up/down
    const offsetY = 3;

    screenX = trackWidth / 2 + (screenX - trackWidth / 2) * scale + offsetX;
    screenY = trackHeight / 2 + (screenY - trackHeight / 2) * scale + offsetY;

    return {
        x: screenX,
        y: screenY
    };
}

//THSI FUNCTION WILL MOVE THE DOT ONCE
//Each time it runs IT START FROM THE PREVIOUS POSITION
function moveDot() {
    //Conditional: If there is not location -> return
    showDtoAtCurrentIndex();

    currentIndex++;

    if (currentIndex >= locationData.length){ //If it exit the range -> return to start
        currentIndex = 0;
    }
}

//Need a function for showing the current point when holding the back/forward buttons
function showDtoAtCurrentIndex() {

    //Conditional: If there is not location -> return
    if (locationData.length === 0){
        return;
    }
    
    if ( currentIndex < 0) {
        currentIndex = 0;
    }

    const point = locationData[currentIndex];
    const scaledPoint = scalePoint(point);
    
    dot.style.left = scaledPoint.x + "px";
    dot.style.top = scaledPoint.y + "px";

    statusText.textContent = `Point ${currentIndex + 1} of ${locationData.length}`;
} 

function startHolding (direction) {
    stopHolding(); //Stop any previous holding interval

    holdSpeed = 1;

    holdIntervalId = setInterval(() => {
        currentIndex += direction * holdSpeed;

        showDotAtCurrentIndex();

        holdSpeed++;

        if (holdSpeed > 50) {
            holdSpeed = 50;
        }
    }, 100);
}

function stopHolding() {
    if (holdIntervalId !== null) {
        clearInterval(holdIntervalId);
        holdIntervalId = null;
    }
    holdSpeed = 1; //Reset speed when stop holding
}

playButton.addEventListener("click", () => { // when click do this !
    if (intervalId !== null){ //Prevent starting multiple interval at once -> if the animation is running, DO NOTHING!
        return;
    }

    //if it hasnt run, run for every 200 MILISECONDS
    intervalId = setInterval(moveDot, 100);
})

pauseButton.addEventListener("click", () => {
    clearInterval(intervalId);
    intervalId = null; //stop , when click play its a new interval
    statusText.textContent = "Paused";
})

loadLocationData();

const backwardButton = document.getElementById("backward-button");
const forwardButton = document.getElementById("forward-button");

backwardButton.addEventListener("mousedown", () => {
    startHolding(-1);
});

forwardButton.addEventListener("mousedown", () => {
    startHolding(1);
});

backwardButton.addEventListener("mouseup", stopHolding);
forwardButton.addEventListener("mouseup", stopHolding);

backwardButton.addEventListener("mouseleave", stopHolding);
forwardButton.addEventListener("mouseleave", stopHolding);

backwardButton.addEventListener("touchstart", () => {
    startHolding(-1);
});

forwardButton.addEventListener("touchstart", () => {
    startHolding(1);
});

backwardButton.addEventListener("touchend", stopHolding);
forwardButton.addEventListener("touchend", stopHolding);

