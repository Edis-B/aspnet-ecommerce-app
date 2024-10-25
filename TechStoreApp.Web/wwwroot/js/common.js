async function redirectIfLoggedOut() {
    if (!isLogged()) {
        var loginUrl = '/Identity/Account/Login'; // Generates the login URL
        window.location.href = loginUrl;
    }
}
async function addImageStockBlocker() {
    let images = document.querySelectorAll('.image-for-product');

    images.forEach(image => {
        let itemStock = image.getAttribute('data-stock');

        if (itemStock == 0) {
            const divElement = document.createElement('div');
            const outOfStockSpan = document.createElement('span');

            divElement.style.borderRadius = '20px';
            divElement.style.backgroundColor = 'black';
            divElement.style.opacity = 0.5;
            divElement.style.display = 'flex';
            divElement.style.justifyContent = 'center';
            divElement.style.alignItems = 'center';

            outOfStockSpan.textContent = 'Out Of Stock';
            outOfStockSpan.style.color = 'white';
            outOfStockSpan.style.fontWeight = 'bold';
            outOfStockSpan.style.position = 'absolute';

            image.parentElement.appendChild(divElement);
            divElement.appendChild(image);

            divElement.appendChild(outOfStockSpan);
        }
    });
}

async function addToCart() {
    const addToCartForms = document.querySelectorAll('.add-to-cart');

    addToCartForms.forEach(form => {
        form.addEventListener('submit', async (event) => {
            redirectIfLoggedOut();
            event.preventDefault();
            const buttonClicked = form.querySelector('button');
            // Disable button
            buttonClicked.style.cursor = 'default';
            buttonClicked.disabled = true;

            try {
                const response = await fetch('/Cart/AddToCart', {
                    'method': 'POST',
                    'headers': {
                        'Content-Type': 'application/json'
                    },
                    'body': JSON.stringify({
                        ProductId: form.elements['productId'].value
                    })
                })

                const data = await response.json();

                console.log(data);

                showPopup(data.message);
                await setCartItemsCount();
                
            } catch (error) {
                console.log(error);
            }

            setTimeout(() => {
                buttonClicked.style.cursor = 'pointer';
                buttonClicked.disabled = false;
            }, 1000);
        });
    });
}

async function showPopup(message) {
    // Style the popup
    const popupDiv = document.createElement('div');
    popupDiv.className = 'cartPopup';
    popupDiv.style.zIndex = '1500';
    popupDiv.style.margin = '40px';
    popupDiv.style.marginBottom = '0px';
    popupDiv.style.height = '100px';
    popupDiv.style.width = '200px';
    popupDiv.style.right = '0';
    popupDiv.style.top = '0';
    popupDiv.style.backgroundColor = 'white';
    popupDiv.style.border = 'solid, 1px';
    popupDiv.style.borderColor = 'black';
    popupDiv.style.borderRadius = '8px';
    popupDiv.style.padding = '10px';
    popupDiv.style.pointerEvents = 'none';


    popupDiv.style.display = 'flex';
    popupDiv.style.justifyContent = 'center';
    popupDiv.style.alignItems = 'center';


    // Insert message
    const pElement = document.createElement('p');
    pElement.textContent = message;

    // Append message to popup
    popupDiv.appendChild(pElement);

    // Append popup to container
    const popupContainer = document.querySelector('#cartPopupsContainer');
    popupContainer.appendChild(popupDiv);

    // Wait 2s before removing it
    setTimeout(() => {
        $(popupDiv).fadeOut(1000, function () {
            // Remove the popup after the fade-out is complete
            popupDiv.remove();
        });
    }, 2000);
}

async function appendPopupContainer() {
    // Create a div for the popups (container)
    const popupDivContainer = document.createElement('div');
    popupDivContainer.id = 'cartPopupsContainer';
    popupDivContainer.style.zIndex = '1500';
    popupDivContainer.style.position = 'fixed';
    popupDivContainer.style.right = '0';
    popupDivContainer.style.top = '0';
    popupDivContainer.style.display = 'flex';
    popupDivContainer.style.flexDirection = 'column';
    popupDivContainer.style.pointerEvents = 'none';

    document.body.appendChild(popupDivContainer);
}

async function areYouSureWindow(message) {
    return new Promise((resolve) => {

        // Dark overlay for the whole screen
        const fullscreenOverlay = document.createElement('div');
        fullscreenOverlay.style.backgroundColor = 'rgba(0, 0, 0, 0.5)';
        fullscreenOverlay.className = 'confirmation-overlay';
        fullscreenOverlay.style.position = 'fixed';
        fullscreenOverlay.style.display = 'block';
        fullscreenOverlay.style.height = '100%';
        fullscreenOverlay.style.width = '100%';
        //fullscreenOverlay.style.zIndex = 2000;
        fullscreenOverlay.style.left = 0;
        fullscreenOverlay.style.top = 0;

        // Create a paragraph for the question
        const questionParagraph = document.createElement('p');
        questionParagraph.style.marginBottom = '20px';
        questionParagraph.style.marginTop = '0px';
        questionParagraph.textContent = message;


        // Create a div for the popups
        const areYouSureDiv = document.createElement('div');
        areYouSureDiv.style.backgroundColor = 'white';
        areYouSureDiv.style.border = '1px solid black';
        areYouSureDiv.style.position = 'fixed'; 
        areYouSureDiv.style.top = '50%';
        areYouSureDiv.style.left = '50%';
        areYouSureDiv.style.transform = 'translate(-50%, -50%)'; 
        areYouSureDiv.style.padding = '20px';
        areYouSureDiv.style.zIndex = '2000';
        areYouSureDiv.style.opacity = 1;
        areYouSureDiv.style.textAlign = 'center';


        const buttonsDiv = document.createElement('div');
        buttonsDiv.style.display = 'flex';
        buttonsDiv.style.flexDirection = 'row';
        buttonsDiv.style.justifyContent = 'space-around';

        const yesButton = document.createElement('button');
        yesButton.style.backgroundColor = 'black';
        yesButton.style.color = 'white';
        yesButton.style.border = '1px solid black';
        yesButton.style.cursor = 'pointer';
        yesButton.style.fontSize = '1.5rem';
        yesButton.textContent = 'yes';
        yesButton.id = 'yes-button';

        // Return true when clicking button
        yesButton.addEventListener('click', () => {
            fullscreenOverlay.remove();
            resolve(true);
        });

        const noButton = document.createElement('button');
        noButton.style.backgroundColor = 'white';
        noButton.style.border = 'black';
        noButton.style.border = '1px solid black';
        noButton.style.cursor = 'pointer';
        noButton.style.fontSize = '1.5rem';
        noButton.textContent = 'no';
        noButton.id = 'no-button';

        // Return false when clicking button
        noButton.addEventListener('click', () => {
            fullscreenOverlay.remove();
            resolve(false);
        })

        // Prevent closing when clicking inside the areYouSureDiv
        areYouSureDiv.addEventListener('click', (event) => {
            event.stopPropagation();
        });

        // Return false if clicked off the popup
        fullscreenOverlay.addEventListener('click', () => {
            fullscreenOverlay.remove();
            resolve(false);
        })

        areYouSureDiv.appendChild(questionParagraph);

        buttonsDiv.appendChild(yesButton);
        buttonsDiv.appendChild(noButton);

        areYouSureDiv.appendChild(buttonsDiv);

        fullscreenOverlay.appendChild(areYouSureDiv);

        document.body.appendChild(fullscreenOverlay);
    });
}
async function addFavoriting() {
    const allCheckBoxes = document.querySelectorAll('.image-checkbox-input');

    allCheckBoxes.forEach((checkBox) => {
        // Set the checkbox checked status based on its value
        if (checkBox.value === "checked") {
            checkBox.checked = true;
        } else {
            checkBox.checked = false;
        }

        checkBox.addEventListener('change', async () => {
            redirectIfLoggedOut();

            let productId = checkBox.id.split('_')[1];
            productId = parseInt(productId, 10);

            // If the checkbox is checked, send a request to add to favorites
            if (checkBox.checked) {
                try {
                    const response = await fetch('/Favorites/AddToFavorites', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json'
                        },
                        body: JSON.stringify({
                            ProductId: productId
                        })
                    });

                    if (!response.ok) {
                        // If response is not OK, throw an error with the status text
                        throw new Error(`HTTP error! status: ${response.status}`);
                    }

                    const data = await response.json();

                    showPopup(data.message);
                } catch (error) {
                    // Handle any errors that occurred during the fetch call
                    console.error('Error adding to favorites:', error);
                }
            }
            // If the checkbox is unchecked, send a request to remove from favorites
            else {
                try {
                    const response = await fetch('/Favorites/RemoveFromFavorites', {
                        method: 'DELETE',
                        headers: {
                            'Content-Type': 'application/json'
                        },
                        body: JSON.stringify({
                            ProductId: productId
                        })
                    });

                    if (!response.ok) {
                        // If response is not OK, throw an error with the status text
                        throw new Error(`HTTP error! status: ${response.status}`);
                    }

                    const data = await response.json();

                    showPopup(data.message);
                } catch (error) {
                    // Handle any errors that occurred during the fetch call
                    console.error('Error removing from favorites:', error);
                }
            }
        });
    });
}

async function setCartItemsCount() {
    const response = await fetch('/Cart/GetCartItemsCount', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    });

    const data = await response.json();
    const itemCount = data.total;

    const span = document.querySelector('#cart-item-count');

    if (!itemCount || itemCount == 0) {
        span.style.display = 'none';
    }
    else {
        span.style.display = 'flex';
        span.textContent = itemCount;
    }
}
