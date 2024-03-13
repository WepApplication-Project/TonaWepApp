const daysTag = document.querySelector(".days"),
currentDate = document.querySelector(".current-date"),
prevNextIcon = document.querySelectorAll(".icons span");

// getting new date, current year and month
let date = new Date(),
currYear = date.getFullYear(),
currMonth = date.getMonth();

// storing full name of all months in array
const months = ["January", "February", "March", "April", "May", "June", "July",
              "August", "September", "October", "November", "December"];

const renderCalendar = () => {
    let firstDayofMonth = new Date(currYear, currMonth, 1).getDay(), // getting first day of month
    lastDateofMonth = new Date(currYear, currMonth + 1, 0).getDate(), // getting last date of month
    lastDayofMonth = new Date(currYear, currMonth, lastDateofMonth).getDay(), // getting last day of month
    lastDateofLastMonth = new Date(currYear, currMonth, 0).getDate(); // getting last date of previous month
    let liTag = "";

    for (let i = firstDayofMonth; i > 0; i--) {
        // Calculate the date for the previous month
        const prevMonthDate = new Date(currYear, currMonth, 0 - i + 1);
        
        // Format the date in "YYYY-MM-DD" format
        const formattedDate = `${prevMonthDate.getFullYear()}-${(prevMonthDate.getMonth() + 1).toString().padStart(2, '0')}-${prevMonthDate.getDate().toString().padStart(2, '0')}`;
        
        liTag += `<li class="inactive" data-date="${formattedDate}">${lastDateofLastMonth - i + 1}</li>`;
    }
    
    for (let i = 1; i <= lastDateofMonth; i++) {
        let isToday = i === date.getDate() && currMonth === new Date().getMonth() && currYear === new Date().getFullYear() ? "active" : "normal";
        liTag += `<li class="${isToday}" data-date="${currYear}-${currMonth + 1}-${i}">${i}</li>`;
    }
    

    for (let i = lastDayofMonth; i < 6; i++) {
        // Calculate the date for the next month
        const nextMonthDate = new Date(currYear, currMonth + 1, i - lastDayofMonth + 1);
        
        // Format the date in "YYYY-MM-DD" format
        const formattedDate = `${nextMonthDate.getFullYear()}-${(nextMonthDate.getMonth() + 1).toString().padStart(2, '0')}-${nextMonthDate.getDate().toString().padStart(2, '0')}`;
        
        liTag += `<li class="inactive" data-date="${formattedDate}">${i - lastDayofMonth + 1}</li>`;
    }
    
    currentDate.innerText = `${months[currMonth]} ${currYear}`; // passing current mon and yr as currentDate text
    daysTag.innerHTML = liTag;
}
renderCalendar();
attachDateListeners();




function attachDateListeners() {
    const dateElements = document.querySelectorAll('.days li');

    dateElements.forEach(dateElement => {
        dateElement.addEventListener('click', () => {
            // Remove the 'selected' class from all date elements
            dateElements.forEach(element => element.classList.remove('selected'));

            // Add the 'selected' class to the clicked date element
            dateElement.classList.add('selected');

            // Get the selected date value from the data-date attribute
            const selectedDate = dateElement.getAttribute('data-date');

            // Show an alert with the selected date


            //alert(`Selected Date: ${selectedDate}`);
        });
    });
}



























prevNextIcon.forEach(icon => {
    icon.addEventListener("click", () => {
        // If clicked icon is previous icon, decrement current month by 1; otherwise, increment it by 1
        currMonth = icon.id === "prev" ? currMonth - 1 : currMonth + 1;

        if (currMonth < 0 || currMonth > 11) {
            // If current month is less than 0 or greater than 11,
            // create a new date of the current year & month and pass it as date value
            date = new Date(currYear, currMonth, new Date().getDate());
            currYear = date.getFullYear();
            currMonth = date.getMonth();
        } else {
            // Otherwise, create a new date based on the current year and month
            date = new Date(currYear, currMonth, new Date().getDate());
        }

        renderCalendar(); // Update the calendar display
        attachDateListeners(); 
    });
});

// Rest of your code...


const optionMenu = document.querySelector(".select-menu"),
       selectBtn = optionMenu.querySelector(".select-btn"),
       options = optionMenu.querySelectorAll(".option"),
       sBtn_text = optionMenu.querySelector(".sBtn-text");

selectBtn.addEventListener("click", () => optionMenu.classList.toggle("active"));       

options.forEach(option =>{
    option.addEventListener("click", ()=>{
        let selectedOption = option.querySelector(".option-text").innerText;
        sBtn_text.innerText = selectedOption;

        optionMenu.classList.remove("active");
    });
});

