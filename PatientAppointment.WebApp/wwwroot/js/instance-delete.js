let patientId;

const deleteModal = document.getElementById("deleteModal");
deleteModal.addEventListener('show.bs.modal', function (event) {
    const button = event.relatedTarget;
    patientId = button.getAttribute('data-patient-id');
    const patientName = button.getAttribute('data-patient-name');
    deleteModal.querySelector("#patientName").textContent = patientName;
});

var confirmButton = deleteModal.querySelector("#confirmButton");
confirmButton.onclick = function () {
    var form = document.getElementById("antiForgeryForm");
    var token = form.querySelector('input[name="__RequestVerificationToken"]').value;
    fetch(`/Patients/Delete/${patientId}`, {
        method: 'POST',
        headers: {
            'RequestVerificationToken': token
        }
    }).then(response => response.json())    
        .then(data => {
            if (data.success) {

                const patientRow = document.getElementById(`patient-row-${patientId}`);
                const patientCard = document.getElementById(`patient-card-${patientId}`);
                if (patientRow) {
                    patientRow.classList.add('fading-out');
                    setTimeout(() => {
                        patientRow.remove();
                    }, 500);

                }
                if (patientCard) {
                    patientCard.classList.add('fading-out');
                    setTimeout(() => {
                        patientCard.remove();
                    }, 500);
                }

                toastr.success(data.message, 'Success', { "closeButton": true, "progressBar": true, "timeOut": 5000 });

                

                const modalInstance = bootstrap.Modal.getInstance(deleteModal);
                modalInstance.hide();
            } else {
                alert("Error deleting patient: " + data.message);
            }
        }).catch(error => {
            console.error("Error deleting patient:", error);
            alert("An error occurred while trying to delete the patient.");
        });
}