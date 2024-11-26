document.addEventListener('DOMContentLoaded', async () => {
    await addressDropDown();
});

async function addressDropDown() {
    // Address inputs
    const inputs = document.querySelectorAll('#sharedForm input');
    const detail = document.querySelector('.address-details');

    // Select address preset
    const select = document.getElementById('adresses');

    // Address selector
    select.addEventListener('change', async () => {
        const option = select.options[select.selectedIndex];
        if (option.value == 0) {
            // Empty when choosing Empty
            inputs.forEach(input => {
                value = '';
            });
        }
        else {
            // Fill when choosing address
            const response = await fetch(`/Address/GetAddress/${option.value}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                }
            })

            const data = await response.json();

            inputs[0].value = data.country;
            inputs[1].value = data.city;
            inputs[2].value = data.postalCode;
            detail.value = data.details;
        }
    })
}