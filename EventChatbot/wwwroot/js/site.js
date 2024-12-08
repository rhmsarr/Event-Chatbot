// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


// script.js
document.addEventListener("DOMContentLoaded", function () {
    const calendarBody = document.getElementById("calendar-body");
    const monthName = document.getElementById("month-name");
    const eventsList = document.getElementById("events-list");
    let currentDate = new Date();

    const staticSpecialDays = [
        { date: "2024-10-29", label: "Republic Day" },
        { date: "2024-11-10", label: "November 10th Atatürk Commemoration Day" },
        { date: "2025-01-01", label: "New Year" },
        { date: "2025-04-23", label: "April 23 National Sovereignty and Children's Day" },
        { date: "2025-05-01", label: "May 1 Labor and Solidarity Day" },
        { date: "2025-05-19", label: "May 19th Commemoration of Atatürk, Youth and Sports Day" },
        { date: "2025-08-30", label: "August 30 Victory Day" },

       
    ];

    let dynamicEvents = [];
    let allEvents = [...staticSpecialDays];

    fetch('/events_data.json')
        .then((response) => response.json())
        .then((data) => {
            dynamicEvents = data;
            allEvents = [...staticSpecialDays, ...dynamicEvents];
            renderCalendar(currentDate);
        })
        .catch((error) => console.error("An error occurred while loading events:", error));

    function renderCalendar(date) {
        const month = date.getMonth();
        const year = date.getFullYear();
        const firstDayOfMonth = new Date(year, month, 1);
        const lastDayOfMonth = new Date(year, month + 1, 0);
        const daysInMonth = lastDayOfMonth.getDate();
        const startDay = (firstDayOfMonth.getDay() + 6) % 7;

        monthName.textContent = `${date.toLocaleString("en-EN", { month: "long" })} ${year}`;
        calendarBody.innerHTML = "";

        let row = document.createElement("tr");
        calendarBody.appendChild(row);

        for (let i = 0; i < startDay; i++) {
            row.innerHTML += '<td class="empty"></td>';
        }

        let cellCount = startDay;
        for (let day = 1; day <= daysInMonth; day++) {
            if (cellCount % 7 === 0) {
                row = document.createElement("tr");
                calendarBody.appendChild(row);
                cellCount = 0;
            }

            const dayCell = document.createElement("td");
            dayCell.classList.add("calendar-day");

            const fullDate = `${year}-${String(month + 1).padStart(2, "0")}-${String(day).padStart(2, "0")}`;
            dayCell.setAttribute("data-date", fullDate);
            dayCell.textContent = day;

            const specialDay = allEvents.find((event) => event.date === fullDate);
            if (specialDay) {
                dayCell.classList.add("special-day");

                // Türüne göre CSS sınıfı ekle
                if (staticSpecialDays.some((event) => event.date === fullDate)) {
                    dayCell.classList.add("static");
                } else {
                    dayCell.classList.add("dynamic");
                }

                dayCell.title = specialDay.label;
            }

            row.appendChild(dayCell);
            cellCount++;
        }

        updateEventsList();
    }

    document.getElementById("prev-month").addEventListener("click", function () {
        currentDate.setMonth(currentDate.getMonth() - 1);
        renderCalendar(currentDate);
    });

    document.getElementById("next-month").addEventListener("click", function () {
        currentDate.setMonth(currentDate.getMonth() + 1);
        renderCalendar(currentDate);
    });

    function updateEventsList() {
        eventsList.innerHTML = "";
        const currentMonth = currentDate.getMonth() + 1;
        const currentYear = currentDate.getFullYear();

        const monthEvents = allEvents.filter((event) => {
            const eventDate = new Date(event.date);
            return eventDate.getMonth() + 1 === currentMonth && eventDate.getFullYear() === currentYear;
        });

        if (monthEvents.length === 0) {
            eventsList.innerHTML = "<p>There are no events planned for this month.</p>";
        } else {
            monthEvents.forEach((event) => {
                const eventItem = document.createElement("div");
                eventItem.classList.add("event-item");

                if (dynamicEvents.includes(event)) {
                    eventItem.innerHTML = `
                        <h4>${event.label}</h4>
                        <p><strong>Date:</strong> ${event.date}</p>
                        <p><strong>Location:</strong> ${event.location || "Unknown"}</p>
                        <p><strong>Time:</strong> ${event.time || "Not specified"}</p>
                        <p><strong>Description:</strong> ${event.description || "Not specified"}</p>
                    `;
                } else {
                    eventItem.innerHTML = `
                        <h4>${event.label}</h4>
                        <p><strong>Date:</strong> ${event.date}</p>
                    `;
                }

                eventsList.appendChild(eventItem);
            });
        }
    }
});
