document.addEventListener('DOMContentLoaded', () => {
    // Address inputs
    const inputs = document.querySelectorAll('#save-address-form input');
    const detailInput = document.querySelector('.address-details');

    // Address hidden outputs
    const outputs = document.querySelectorAll('#send-address-form input');

    // Both Forms
    const formSave = document.querySelector('#save-address-form');
    const formSend = document.querySelector('#send-address-form');

    // Select address preset
    const select = document.getElementById('adresses');

    formSave.addEventListener('submit', async (event) => {
        event.preventDefault();
        let valid = true;

        if (!inputs[0].value) {
            inputs[0].nextElementSibling.textContent = 'Country cannot be empty!';
            valid = false;
        }
        if (!inputs[1].value) {
            inputs[1].nextElementSibling.textContent = 'City cannot be empty!';
            valid = false;
        }
        if (!inputs[2].value) {
            inputs[2].nextElementSibling.textContent = 'Postal Code cannot be empty';
            valid = false;
        }
        if (!detailInput.value) {
            detailInput.nextElementSibling.textContent = 'Address cannot be empty!';
            valid = false;
        }

        if (valid) {
            formSave.submit();
        }
    })

    formSend.addEventListener('submit', async function(event) {
        event.preventDefault();
        let valid = true;

        if (!inputs[0].value) {
            inputs[0].nextElementSibling.textContent = 'Country cannot be empty!';
            valid = false;
        }
        if (!inputs[1].value) {
            inputs[1].nextElementSibling.textContent = 'City cannot be empty!';
            valid = false;
        }
        if (!inputs[2].value) {
            inputs[2].nextElementSibling.textContent = 'Postal Code cannot be empty';
            valid = false;
        }
        if (!detailInput.value) {
            detailInput.nextElementSibling.textContent = 'Address cannot be empty!';
            valid = false;
        }

        if (valid) {
            // Attach address data to model
            outputs[0].value = inputs[0].value;
            outputs[1].value = inputs[1].value;
            outputs[2].value = inputs[2].value;
            outputs[3].value = detailInput.value;

            formSend.submit();
        }
    });


    // Address selector
    select.addEventListener('change', () => {
        const option = select.options[select.selectedIndex];
        if (option.id == 0) {
            // Empty when choosing Empty
            inputs.forEach(input => {
                input.value = '';
            });
        }
        else {
            // Fill when choosing address
            fetch(`/Order/GetAddress/${option.id}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
            .then(response => response.json())
            .then(data => {
                // Populate fields
                inputs[0].value = data.country;
                inputs[1].value = data.city;
                inputs[2].value = data.postalCode;
                detailInput.value = data.details;
            });
        }
    })
});