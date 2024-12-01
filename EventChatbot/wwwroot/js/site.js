// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


// script.js
document.addEventListener("DOMContentLoaded", function () {
    const calendarBody = document.getElementById("calendar-body");
    const monthName = document.getElementById("month-name");
    const eventsList = document.getElementById("events-list");
    let currentDate = new Date();

    function renderCalendar(date) {
        const month = date.getMonth(); // Mevcut ay
        const year = date.getFullYear(); // Mevcut yıl
        const firstDayOfMonth = new Date(year, month, 1); // Ayın ilk günü
        const lastDayOfMonth = new Date(year, month + 1, 0); // Ayın son günü
        const daysInMonth = lastDayOfMonth.getDate(); // Ayda toplam kaç gün var
        const startDay = (firstDayOfMonth.getDay() + 6) % 7; // Pazartesi'den başlatma

        // Ay ismini güncelle
        monthName.textContent = `${date.toLocaleString("en-EN", { month: "long" })} ${year}`;

        // Takvimdeki önceki içeriği temizle
        calendarBody.innerHTML = "";

        // Takvim satırlarını başlat
        let row = document.createElement("tr");
        calendarBody.appendChild(row);

        // Boş hücreler (önceki ayın son gününden kalan boşluk)
        for (let i = 0; i < startDay; i++) {
            row.innerHTML += '<td class="empty"></td>';
        }

        // Günleri ekle
        let cellCount = startDay; // İlk hücreden başlanacak
        for (let day = 1; day <= daysInMonth; day++) {
            // Eğer 7. gün gelirse, yeni bir satır başlat
            if (cellCount % 7 === 0) {
                row = document.createElement("tr"); // Yeni bir satır oluştur
                calendarBody.appendChild(row);
                cellCount = 0; // Hücre sayısını sıfırla
            }

            // Günleri td olarak ekle
            const dayCell = document.createElement("td");
            dayCell.classList.add("calendar-day");
            dayCell.setAttribute("data-date", `${year}-${month + 1}-${day}`);
            dayCell.textContent = day;
            row.appendChild(dayCell);

            cellCount++; // Hücre sayısını arttır
        }

        // Özel günleri işaretle
        markSpecialDays();
        updateEventsList();
    }

    // Ayları değiştirmek için butonları ekle
    document.getElementById("prev-month").addEventListener("click", function () {
        currentDate.setMonth(currentDate.getMonth() - 1);
        renderCalendar(currentDate);
    });

    document.getElementById("next-month").addEventListener("click", function () {
        currentDate.setMonth(currentDate.getMonth() + 1);
        renderCalendar(currentDate);
    });

    // Takvimi ilk kez render et
    renderCalendar(currentDate);
});

// Özel günleri saklayan dizi
const specialDays = [
    { date: "2024-10-29", label: "Republic Day" },
    { date: "2024-11-10", label: "November 10th Atatürk Commemoration Day" },
    { date: "2025-1-1", label: "New Year" },
];

// Özel günleri işaretle
function markSpecialDays() {
    const cells = document.querySelectorAll(".calendar-day");
    cells.forEach((cell) => {
        const date = cell.dataset.date; // Hücredeki tarihi al
        // Özel günlerin tarihleri ile karşılaştırma yap
        const specialDay = specialDays.find((day) => day.date === date);
        if (specialDay) {
            cell.classList.add("special-day"); // Özel günleri işaretle
            cell.title = specialDay.label; // Açıklama ekle
        }
    });
}

// Etkinlik listesini güncelle
function updateEventsList() {
    const eventsList = document.getElementById("events-list");

    // Mevcut listeyi temizle
    eventsList.innerHTML = "";

    // Özel günlerden etkinlikleri ekle
    if (specialDays.length === 0) {
        eventsList.innerHTML = "<p>No events yet. Add special days to populate events here.</p>";
    } else {
        specialDays.forEach((event) => {
            const eventItem = document.createElement("p");
            eventItem.textContent = `${event.date}: ${event.label}`;
            eventsList.appendChild(eventItem);
        });
    }
}

// Özel gün ekleme işlevi
document.getElementById("add-special-day").addEventListener("click", function () {
    const dateInput = document.getElementById("special-date").value;
    const labelInput = document.getElementById("special-label").value;

    if (dateInput && labelInput) {
        // Yeni özel günü diziye ekle
        specialDays.push({ date: formatDateString(dateInput), label: labelInput });

        // Takvimi yeniden oluştur
        renderCalendar(currentDate);

        // Formu temizle
        document.getElementById("special-date").value = "";
        document.getElementById("special-label").value = "";
    } else {
        alert("Please enter a valid date and description.");
    }
});

// Tarih biçimlendirme işlevi
function formatDateString(dateString) {
    const date = new Date(dateString);
    return `${date.getFullYear()}-${date.getMonth() + 1}-${date.getDate()}`;
}




