    const datePicker = document.getElementById('datePicker');
    const dateHeader = document.getElementById('dateHeader');
    const timeSlotsContainer = document.getElementById('timeSlots');

    // Generate time slots for a given date
    function generateTimeSlots(selectedDate) {
        timeSlotsContainer.innerHTML = "";
    let startHour = 8;
    let endHour = 23; // 10 PM
    let minutesStep = 30;

    for (let hour = startHour; hour < endHour; hour++) {
                for (let min = 0; min < 60; min += minutesStep) {
        let dateTime = new Date(selectedDate);
    dateTime.setHours(hour, min, 0, 0);

    let timeStr = dateTime.toLocaleTimeString([], {hour: '2-digit', minute: '2-digit' });
    let dateStr = dateTime.toISOString(); // full datetime for backend

    let col = document.createElement('div');
    col.className = "col-6 col-md-4 col-lg-2";

    let card = document.createElement('div');
    card.className = "time-slot";
    card.setAttribute('data-datetime', dateStr);
    card.innerHTML = `<strong>${timeStr}</strong><br><small>${dateStr}</small>`;

        card.addEventListener('click', function() {
            document.querySelectorAll('.time-slot').forEach(c => c.classList.remove('selected'));
        this.classList.add('selected');
        console.log("Selected slot:", this.dataset.datetime);
                    });

        col.appendChild(card);
        timeSlotsContainer.appendChild(col);
                }
            }
        }

        // Update header with date + day name
        function updateDateHeader(date) {
            const dayName = date.toLocaleDateString('en-US', {weekday: 'long' });
        const dateStr = date.toLocaleDateString('en-US', {year: 'numeric', month: 'long', day: 'numeric' });
        dateHeader.textContent = `${dayName}, ${dateStr}`;
        }

        // Initialize with today's date
        let today = new Date();
        datePicker.value = today.toISOString().split('T')[0];
        updateDateHeader(today);
        generateTimeSlots(today);

        // When date changes
        datePicker.addEventListener('change', function() {
            let selected = new Date(this.value);
        updateDateHeader(selected);
        generateTimeSlots(selected);
        });
