
let form = document.getElementById("antiForgeryForm");
let token = form.querySelector('input[name="__RequestVerificationToken"]').value;
let appointmentCards = document.querySelectorAll('.time-slot-booked');
let menu = document.getElementById('contextMenu');
let appointmentId;
let appointmentStatus;
let liStatusDisplay = 'li-scheduled';
let allMenuItems = menu.querySelectorAll('li');
let timeSlots = document.getElementById("timeSlots");

timeSlots.addEventListener('dblclick', function (event) {
    event.preventDefault();
    const card = event.target.closest('.time-slot')
    if (!card) {
        return;
    }
    const action = card.dataset.action;
    const dateTime = card.dataset.dateTime;
    console.log(dateTime);


    if (action === 'create') {
        fetch('/Appointments/QuickCreate/', {
            method: 'GET'
        }).then(response => response.text()).then(data => {
            const modalLabel = document.getElementById('createModalLabel');
            const modalBody = document.getElementById('createModalBody');
            modalLabel.innerText = "Create New Appointment";
            modalBody.innerHTML = data;
            const myModalElement = document.getElementById('createModal');
            const modal = bootstrap.Modal.getOrCreateInstance(myModalElement);
            modal.show();
            const dateTimeInput = document.querySelector('input[name="StartDateTime"]');
            dateTimeInput.value = dateTime;
            const submitButton = document.getElementById('submitButton');
            submitButton.addEventListener('click', function () {

                const quickAddForm = document.getElementById('quickAddForm');
                const patientId = quickAddForm.querySelector('[name="PatientId"]').value;
                const appointmentType = quickAddForm.querySelector('[name="AppointmentType"]').value;
                const startDateTime = quickAddForm.querySelector('[name="StartDateTime"]').value;
                const formToken = quickAddForm.querySelector('[name="__RequestVerificationToken"]').value;

                const appointmentData = {
                    "PatientId": parseInt(patientId),
                    "AppointmentType": parseInt(appointmentType),
                    "StartDateTime": startDateTime
                }
                const stringappointmentData = JSON.stringify(appointmentData);

                fetch('/Appointments/QuickCreate/', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'RequestVerificationToken': formToken
                    },
                    body: stringappointmentData
                }).then(response => response.json()).then(data => {
                    if (data.success) {
                        toastr.success(data.message, 'Success', { "closeButton": true, "progressBar": true, "timeOut": 5000 });
                        setTimeout(() => {
                            location.reload();
                        }, 1000);
                    } else {
                        const validationSpanAppointmentType = document.querySelector('[data-valmsg-for="AppointmentType"]');
                        const appointmentElement = document.getElementById("AppointmentType");
                        if (appointmentType === "") {
                            validationSpanAppointmentType.innerText = appointmentElement.getAttribute('data-val-required');
                        }
                        else {
                            validationSpanAppointmentType.innerText = ""
                        }
                        const validationSpanPatient = document.querySelector('[data-valmsg-for="PatientId"]');
                        const patientElement = document.getElementById("PatientId");
                        if (patientId === "") {
                            validationSpanPatient.innerText = patientElement.getAttribute('data-val-required');
                        } else {
                            validationSpanPatient.innerText = ""
                        }
                    }
                }).catch(error => {
                    console.error("Error creating appointment:", error);
                })


            });
        });





    }
    if (action === 'edit') {
        appointmentId = card.dataset.id;
        fetch(`/Appointments/QuickUpdate/${appointmentId}`, {
            method: 'GET'
        }).then(response => response.text()).then(data => {
            const modalLabel = document.getElementById('createModalLabel');
            const modalBody = document.getElementById('createModalBody');
            modalLabel.innerText = "Update Appointment";
            modalBody.innerHTML = data;
            const myModalElement = document.getElementById('createModal');
            const modal = bootstrap.Modal.getOrCreateInstance(myModalElement);
            modal.show();
            const dateTimeInput = document.querySelector('input[name="StartDateTime"]');
            dateTimeInput.value = dateTime;
            const IdInput = document.querySelector('input[name="Id"]');
            IdInput.value = appointmentId;

            const submitButton = document.getElementById('submitButton');
            submitButton.addEventListener('click', function () {

                const quickAddForm = document.getElementById('quickAddForm');

                const patientSelect = quickAddForm.querySelector('[name="PatientId"]');
                const patientId = patientSelect.value;
                const PatientName = patientSelect.options[patientSelect.selectedIndex].text;

                const appointmentTypeSelect = quickAddForm.querySelector('[name="AppointmentType"]');
                const appointmentTypeValue = appointmentTypeSelect.value;
                const appointmentTypeText = appointmentTypeSelect.options[appointmentTypeSelect.selectedIndex].text;

                const appointmentStatusSelect = quickAddForm.querySelector('[name="AppointmentStatus"]');
                const appointmentStatusValue = appointmentStatusSelect.value;
                const appointmentStatusText = appointmentStatusSelect.options[appointmentStatusSelect.selectedIndex].text;
                const appointmentStatusClass = appointmentStatusText.charAt(0).toLowerCase() + appointmentStatusText.slice(1);
                const startDateTime = quickAddForm.querySelector('[name="StartDateTime"]').value;
                const formToken = quickAddForm.querySelector('[name="__RequestVerificationToken"]').value;

                const appointmentData = {
                    "Id": parseInt(appointmentId),
                    "PatientId": parseInt(patientId),
                    "AppointmentType": parseInt(appointmentTypeValue),
                    "StartDateTime": startDateTime,
                    "AppointmentStatus": parseInt(appointmentStatusValue)
                }
                const stringappointmentData = JSON.stringify(appointmentData);
                fetch('/Appointments/QuickUpdate/', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'RequestVerificationToken': formToken
                    },
                    body: stringappointmentData
                }).then(response => response.json()).then(data => {
                    if (data.success) {
                        const appointmentCard = document.getElementById(`appointment-card-${appointmentId}`);
                        appointmentCard.classList.add('fading-out');
                        document.getElementById(`appointment-name-${appointmentId}`).textContent = PatientName;
                        document.getElementById(`appointment-type-${appointmentId}`).textContent = appointmentTypeText;
                        document.getElementById(`appointment-status-${appointmentId}`).textContent = appointmentStatusText;
                        appointmentCard.classList = `card time-slot time-slot-booked status-${appointmentStatusClass} p-0`;
                        appointmentCard.dataset.status = appointmentStatusClass;
                        modal.hide();
                        toastr.success(data.message, 'Success', { "closeButton": true, "progressBar": true, "timeOut": 5000 });



                       
                    }
                })
            });


        });
    }

});


appointmentCards.forEach(card => {
    card.addEventListener('contextmenu', function (event) {
        event.preventDefault();
        menu.style.display = "block";
        menu.style.left = event.pageX + "px";
        menu.style.top = event.pageY + "px";
        appointmentId = card.dataset.id;

        // Hide the menu item corresponding to the current appointment status
        allMenuItems.forEach(item => {
            item.hidden = false;
        });
        appointmentStatus = card.dataset.status;
        liStatusDisplay = document.getElementById(`li-${appointmentStatus}`);
        liStatusDisplay.hidden = true;

    })
});

menu.addEventListener('click', function (event) {
    const action = event.target.dataset.action;
    if (action === 'updateStatus') {

        const newStatus = event.target.dataset.status;
        const CapitzlizeNewStatus = newStatus.charAt(0).toUpperCase() + newStatus.slice(1);
        fetch(`/Appointments/UpdateStatus?id=${appointmentId}&newStatus=${CapitzlizeNewStatus}`, {
            method: 'POST',
            headers: {
                'RequestVerificationToken': token
            }
        }).then(response => {

            console.log(response);
            return response.json();
        }).then(data => {
            if (data.success) {
                const appointmentCard = document.getElementById(`appointment-card-${appointmentId}`);
                console.log(appointmentCard);
                appointmentCard.dataset.status = newStatus;
                appointmentCard.querySelector('#appointment-status-' + appointmentId).textContent = newStatus.charAt(0).toUpperCase() + newStatus.slice(1);
                appointmentCard.className = `card time-slot time-slot-booked status-${newStatus} p-0`;
                toastr.success(data.message, 'Success', { "closeButton": true, "progressBar": true, "timeOut": 5000 });

            } else {
                console.error("Error updating status: " + data.message);

            }
        }).catch(error => {
            console.error("Error deleting patient:", error);
        })

    }

    if (action === 'delete') {
        const modalLabel = document.getElementById('createModalLabel');
        const modalBody = document.getElementById('createModalBody');
        modalLabel.innerText = "Delete";
        modalBody.innerHTML = "Are you sure you want to delete appointment?";
        const myModalElement = document.getElementById('createModal');
        const deleteArea = document.getElementById("deleteArea");
        deleteArea.hidden = false;
        const modal = bootstrap.Modal.getOrCreateInstance(myModalElement);
        modal.show();
        const submitButton = document.getElementById('submitButton');
        submitButton.addEventListener('click', function () {
            fetch(`/Appointments/Delete/${appointmentId}`, {
                method: 'POST',
                headers: {

                    'RequestVerificationToken': token
                }
            }).then(response => response.json()).then(data => {
                if (data.success) {
                    const appointmentCard = document.getElementById(`appointment-card-${appointmentId}`);
                    const appointmentTime = appointmentCard.querySelector(`#appointment-time-${appointmentId}`).innerText;
                    appointmentCard.dataset.action = 'create';
                    appointmentCard.className = 'card time-slot time-slot-available p-4';
                    appointmentCard.removeAttribute('data-id');
                    appointmentCard.removeAttribute('data-status');
                    appointmentCard.removeAttribute('id');
                    appointmentCard.innerHTML = `
                            <div class="card-body text-center" >
                                <strong>${appointmentTime} </strong ><br />
                                <small class="text-muted">Available</small>
                                <br />
                            </div>  `;
                    
                    modal.hide();
                    deleteArea.hidden = true;
                    toastr.success(data.message, 'Success', { "closeButton": true, "progressBar": true, "timeOut": 5000 });
 
                }
            });
        });

    }
});

document.addEventListener("click", function () {
    menu.style.display = "none";
    liStatusDisplay.hidden = false; //Show Li elemnt after context menu is closed.

});