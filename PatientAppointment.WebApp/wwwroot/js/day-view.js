const datePicker = document.getElementById('datePicker');
const dateHeader = document.getElementById('dateHeader');
const timeSlotsContainer = document.getElementById('timeSlots');

// This is the updated function with the fetch call integrated.
function generateTimeSlots(selectedDate) {
    timeSlotsContainer.innerHTML = "";

    // Format the date into "YYYY-MM-DD" to send to the server
    const dateString = selectedDate.toISOString().split('T')[0];

    // --- NEW: Start the Ajax fetch call ---
    fetch(`/Appointments/GetAppointmentsByDate?date=${dateString}`)
        .then(response => response.json())
        .then(bookedAppointments => {

            // Log the data to the console to confirm it's working
            console.log("Booked appointments received:", bookedAppointments);

            // --- Your original grid generation logic now goes inside here ---
            let startHour = 8;
            let endHour = 23;
            let minutesStep = 30;

            // Replace your existing 'for' loops with this block
            for (let hour = startHour; hour < endHour; hour++) {
                for (let min = 0; min < 60; min += minutesStep) {
                    let dateTime = new Date(selectedDate);
                    dateTime.setHours(hour, min, 0, 0);

                    // --- NEW LOGIC STARTS HERE ---

                    // 1. Check if an appointment from the server matches this specific time slot
                    const matchingAppointment = bookedAppointments.find(appt =>
                        new Date(appt.startDateTime).getTime() === dateTime.getTime()
                    );

                    let col = document.createElement('div');
                    col.className = "col-6 col-md-4 col-lg-2";

                    let card = document.createElement('div');
                    let cardContent = ''; // We'll build the content here

                    // 2. If a match is found, create a "booked" card
                    if (matchingAppointment) {
                        // Add a CSS class based on the appointment's status (e.g., 'status-scheduled')
                        card.className = `time-slot status-${matchingAppointment.appointmentStatus.toLowerCase()}`;
                        // Set the content to the patient's name and appointment type
                        cardContent = `<strong>${matchingAppointment.patient.fullName}</strong><br><small>${matchingAppointment.appointmentType}</small>`;
                        // Store the appointment ID for later use (editing, deleting)
                        card.setAttribute('data-appointment-id', matchingAppointment.id);
                    }
                    // 3. If no match, create an "available" card
                    else {
                        card.className = "time-slot status-available";
                        let timeStr = dateTime.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
                        cardContent = `<strong>${timeStr}</strong><br><small>Available</small>`;
                    }

                    // --- END OF NEW LOGIC ---

                    card.innerHTML = cardContent;
                    card.setAttribute('data-datetime', dateTime.toISOString());

                    card.addEventListener('click', function () {
                        document.querySelectorAll('.time-slot').forEach(c => c.classList.remove('selected'));
                        this.classList.add('selected');
                    });

                    col.appendChild(card);
                    timeSlotsContainer.appendChild(col);
                }
            }

        })
        .catch(error => {
            console.error('Error fetching appointments:', error);
            // Optionally, display an error message to the user in the UI
            timeSlotsContainer.innerHTML = `<div class="alert alert-danger">Could not load appointments. Please try again.</div>`;
        });
}

// This function does not need to change
function updateDateHeader(date) {
    const dayName = date.toLocaleDateString('en-US', { weekday: 'long' });
    const dateStr = date.toLocaleDateString('en-US', { year: 'numeric', month: 'long', day: 'numeric' });
    dateHeader.textContent = `${dayName}, ${dateStr}`;
}

// This initialization code does not need to change
let today = new Date();
datePicker.value = today.toISOString().split('T')[0];
updateDateHeader(today);
generateTimeSlots(today);

// This event listener does not need to change
datePicker.addEventListener('change', function () {
    // We need to get the date value correctly, accounting for timezone offset
    const selectedDateParts = this.value.split('-');
    const selected = new Date(selectedDateParts[0], selectedDateParts[1] - 1, selectedDateParts[2]);
    updateDateHeader(selected);
    generateTimeSlots(selected);
});