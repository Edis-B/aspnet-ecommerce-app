document.addEventListener("DOMContentLoaded", async () => {
    appendPopupContainer();

    addToCart();
    
    removeFromFavorites();
});

async function removeFromFavorites() {
    const forms = document.querySelectorAll('.remove-from-favorites');

    forms.forEach(form => {
        form.addEventListener('submit', async function (event) {
            event.preventDefault();

            const confirmation = await areYouSureWindow('Are you sure you want to remove this product from your favorites?');

            if (confirmation) {
                // Get the id of the element
                // Testing commits
                const containerDiv = form.closest(`.row-container`);
                productId = containerDiv.id.split('_')[1];

                const response = await fetch('/Favorites/RemoveFromFavorites', {
                    method: 'DELETE',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({
                        ProductId: productId
                    })
                });

                const data = await response.json();

                showPopup(data.message);
                containerDiv.remove();
            }
        });
    });
}