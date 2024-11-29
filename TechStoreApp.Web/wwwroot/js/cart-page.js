document.addEventListener('DOMContentLoaded', async function () {
    appendPopupContainer();

    increaseCount();
    decreaseCount();

    trashCans();
    clearCartForm();
});
async function clearCartForm() {
    const form = document.getElementById('clear-cart-form');

    form.addEventListener('submit', async function (event) {
        event.preventDefault();

        const confirmation = await areYouSureWindow('Are you sure you want to clear your cart?');

        if (confirmation) {
            const response = await fetch('/Cart/ClearCart', {
                'method': 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
            });

            const data = await response.json();

            showPopup(data.message);
            setCartItemsCount();

            location.reload();
        }
    });
}

async function trashCans() {
    const forms = document.querySelectorAll('.remove-from-cart-form');

    forms.forEach(form => {
        form.addEventListener('submit', async function (event) {
            try {
                event.preventDefault();

                const confirmation = await areYouSureWindow('Are you sure you want to remove this product from your cart?');

                if (confirmation) {

                    const productDiv = form.closest('.product-div');

                    const response = await fetch('/Cart/RemoveFromCart', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json'
                        },
                        body: JSON.stringify({
                            ProductId: productDiv.id
                        })
                    });

                    const data = await response.json();

                    // Show popup and update cart count
                    showPopup(data.message);
                    setCartItemsCount();

                    //Find and remove product div
                    const productDiv = document.querySelector(`.product-div[id="${decreaseCountBtn.value}"]`);
                    productDiv.nextElementSibling.remove();
                    productDiv.remove();
                }
            } catch (error) {
                console.log(error);
            }
        });
    });
}

async function increaseCount() {
    const increaseCountBtns = document.querySelectorAll('.increase-count-class');

    increaseCountBtns.forEach(increaseCountBtn => {
        increaseCountBtn.addEventListener('click', async () => {
            try {
                // Disable button to prevent spam
                increaseCountBtn.disabled = true;
                increaseCountBtn.style.cursor = 'default';

                const itemContainer = document.getElementById(increaseCountBtn.value);
                const quantityDiv = itemContainer.querySelector('.item-quantity');

                const response = await fetch('/Cart/IncreaseCount', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({
                        ProductId: increaseCountBtn.value
                    })
                })

                const data = await response.json();
                console.log(data);

                // Set the new quantity
                quantityDiv.textContent = data.newQuantity;

                // Show popup and update cart count
                showPopup(data.message);
                setCartItemsCount();

                // Wait 1s before enabling the button again
                setTimeout(() => {
                    increaseCountBtn.disabled = false;
                    increaseCountBtn.style.cursor = 'pointer';
                }, 1000);
            } catch (error) {
                console.log(error);
            }
            
        });
    });
}

async function decreaseCount() {
    const decreaseCountBtns = document.querySelectorAll('.decrease-count-class');

    decreaseCountBtns.forEach(decreaseCountBtn => {
        decreaseCountBtn.addEventListener('click', async () => {
            // Disable button to prevent spam
            decreaseCountBtn.disabled = true;
            decreaseCountBtn.style.cursor = 'default';
             
            const itemContainer = document.getElementById(decreaseCountBtn.value);
            const quantityDiv = itemContainer.querySelector('.item-quantity');

            // If there is 1 product, ask if they want to delete it
            if (quantityDiv.textContent == '1') {

                const answer = await areYouSureWindow('Are you sure you want to remove this item from your cart?');

                if (answer) {
                    try {
                        const response = await fetch('/Cart/RemoveFromCart', {
                            method: 'POST',
                            headers: {
                                'Content-Type': 'application/json'
                            },
                            body: JSON.stringify({ ProductId: decreaseCountBtn.value })
                        })

                        const data = await response.json();
                        console.log(data);

                        // Show popup and update cart count
                        showPopup(data.message);
                        setCartItemsCount();

                        // Find and remove product div
                        const productDiv = decreaseCountBtn.closest('.product-div');
                        productDiv.remove();

                    } catch (error) {
                        console.log(error)
                    }
                }
            // Else decrease it normally
            } else {
                try {
                    const response = await fetch('/Cart/DecreaseCount', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json'
                        },
                        body: JSON.stringify({ ProductId: decreaseCountBtn.value })
                    })

                    const data = await response.json();
                    console.log(data);

                    // Set the new quantity
                    quantityDiv.textContent = data.newQuantity;

                    // Show popup and update cart count
                    showPopup(data.message);
                    setCartItemsCount();
                } catch (error) {
                    console.log(error);
                }
            }

            // Wait 1s before enabling the button again
            setTimeout(() => {
                decreaseCountBtn.style.cursor = 'pointer';
                decreaseCountBtn.disabled = false;
            }, 1000);
        });
    });
}