// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


// script.js
document.addEventListener("DOMContentLoaded", function () {
    const calendarBody = document.getElementById("calendar-body");
    const monthName = document.getElementById("month-name");
    const eventsList = document.getElementById("events-list");
    let currentDate = new Date();

    // Sabit etkinlikler (her yıl aynı tarihlerde)
    const staticSpecialDays = [
        { date: "10-29", label: "Republic Day" },
        { date: "11-10", label: "November 10th Atatürk Commemoration Day" },
        { date: "01-01", label: "New Year" },
        { date: "04-23", label: "April 23 National Sovereignty and Children's Day" },
        { date: "05-01", label: "May 1 Labor and Solidarity Day" },
        { date: "05-19", label: "May 19th Commemoration of Atatürk, Youth and Sports Day" },
        { date: "08-30", label: "August 30 Victory Day" },
    ];

    let dynamicEvents = [];
    let allEvents = [];

    fetch('/events_data.json')
        .then((response) => response.json())
        .then((data) => {
            dynamicEvents = data;
            allEvents = [...dynamicEvents];
            renderCalendar(currentDate);
        })
        .catch((error) => console.error("An error occurred while loading events:", error));

    // Takvimi render etme fonksiyonu
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

            // Sabit etkinlikleri her yıl için hesaplayıp takvime eklemek için
            const staticEventDate = `${String(month + 1).padStart(2, "0")}-${String(day).padStart(2, "0")}`;
            const staticEvent = staticSpecialDays.find(event => event.date === staticEventDate);
            if (staticEvent) {
                dayCell.classList.add("special-day", "static");
                dayCell.title = staticEvent.label;
            }

            // Dinamik etkinlikleri kontrol et
            const dynamicEvent = dynamicEvents.find(event => event.date === fullDate);
            if (dynamicEvent) {
                dayCell.classList.add("special-day", "dynamic");
                dayCell.title = dynamicEvent.label;
            }

            row.appendChild(dayCell);
            cellCount++;
        }

        updateEventsList();
    }

    // Sabit günleri her yıl için eklememek için
    function updateStaticSpecialDays(year) {
        return staticSpecialDays.map((event) => {
            // Sabit etkinliklerin yılını ekleyerek her yıl için etkinlikleri oluşturur
            return { 
                date: `${year}-${event.date}`, 
                label: event.label 
            };
        });
    }

    // Önceki ve sonraki aylar için butonlar
    document.getElementById("prev-month").addEventListener("click", function () {
        currentDate.setMonth(currentDate.getMonth() - 1);
        renderCalendar(currentDate);
    });

    document.getElementById("next-month").addEventListener("click", function () {
        currentDate.setMonth(currentDate.getMonth() + 1);
        renderCalendar(currentDate);
    });

    // Etkinlikler listesini güncelleme fonksiyonu
    function updateEventsList() {
        eventsList.innerHTML = "";
        const currentMonth = currentDate.getMonth() + 1;
        const currentYear = currentDate.getFullYear();

        // Sabit günleri her yıl ekle
        const staticEventsForThisYear = updateStaticSpecialDays(currentYear);

        // Dinamik etkinlikler ve sabit etkinlikler birleştiriliyor
        const monthEvents = [...staticEventsForThisYear, ...dynamicEvents].filter((event) => {
            const eventDate = new Date(event.date);
            return eventDate.getMonth() + 1 === currentMonth && eventDate.getFullYear() === currentYear;
        });

        if (monthEvents.length === 0) {
            eventsList.innerHTML = "<p>There are no events planned for this month.</p>";
        } else {
            const eventDates = new Set();  // Aynı tarihteki etkinliklerin tekrarını engellemek için
            monthEvents.forEach((event) => {
                const eventDate = event.date.split("-")[1] + "-" + event.date.split("-")[2];  // Yıl hariç gün ve ay
                if (!eventDates.has(eventDate)) {
                    eventDates.add(eventDate);

                    const eventItem = document.createElement("div");
                    eventItem.classList.add("event-item");

                    eventItem.innerHTML = `
                        <h4>${event.label}</h4>
                        <p><strong>Date:</strong> ${event.date}</p>
                        <p><strong>Location:</strong> ${event.location || "Unknown"}</p>
                        <p><strong>Time:</strong> ${event.time || "Not specified"}</p>
                        <p><strong>Description:</strong> ${event.description || "Not specified"}</p>
                    `;
                    eventsList.appendChild(eventItem);
                }
            });
        }
    }

    // "Add Special Day" butonuna tıklandığında özel gün eklemek
    document.getElementById("add-special-day").addEventListener("click", function () {
        const dateInput = document.getElementById("special-date").value;
        const labelInput = document.getElementById("special-label").value;

        if (dateInput && labelInput) {
            const newEvent = {
                date: dateInput,
                label: labelInput
            };

            // Dinamik etkinliklere ekle
            dynamicEvents.push(newEvent);

            // Tüm etkinlikleri güncelle
            allEvents = [...dynamicEvents];

            // Takvimi yeniden render et
            renderCalendar(currentDate);

            // Etkinlikler listesini güncelle
            updateEventsList();

            // Formu temizle
            document.getElementById("special-date").value = "";
            document.getElementById("special-label").value = "";
        } else {
            alert("Please enter both date and label.");
        }
    });
});
