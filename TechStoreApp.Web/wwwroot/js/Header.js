document.addEventListener('DOMContentLoaded', () => {
    menuInitialization();

    setCartItemsCount();

    attachCSRFToAjax();
});

function menuInitialization() {
    const burgerButton = document.querySelector('#menu-toggle');
    let popupCategories = document.querySelector('.menu-popup-div');

    burgerButton.addEventListener('change', () => {
        let burgerBackground = burgerButton.parentNode;

        // Add animations for burger menu
        if (burgerButton.checked) {
            popupCategories.style.display = 'block';
            burgerBackground.style.backgroundColor = 'white';
            overlay.style.display = 'block';
        } else {
            popupCategories.style.display = 'none';
            burgerBackground.style.backgroundColor = 'black';
            overlay.style.display = 'none';
        }
    });

    // Overlay to darken the page
    const overlay = document.getElementById('overlay');
    const categories = document.querySelectorAll('.menu-popup-div ul li');

    categories.forEach(category => {
        category.addEventListener('click', async () => {
            let categoryString = category.id;

            window.location.href = `/Search/Search?Category=${categoryString}`;
        });
    });
}

function attachCSRFToAjax() {
    const csrfToken = document.querySelector('input[name="__RequestVerificationToken"]');

    if (csrfToken) {
        const originalFetch = window.fetch;
        window.fetch = function (url, options = {}) {
            options.headers = options.headers || {};
            options.headers['X-CSRF-TOKEN'] = csrfToken.value;

            return originalFetch(url, options);
        };
    }
}

