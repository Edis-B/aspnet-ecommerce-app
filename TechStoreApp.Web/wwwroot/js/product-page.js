document.addEventListener('DOMContentLoaded', function () {
    appendPopupContainer();

    addToCart();
    addFavoriting();

    starAnimations();
    addImageStockBlocker();
});

async function starAnimations() {
    const allStars = document.querySelectorAll('.single-star');

    let i = 1;
    allStars.forEach(star => {
        star.id = i++;

        star.addEventListener('click', () => {
            const reviewInput = document.getElementById('userRating');

            for (let j = 1; j <= star.id; j++) {
                const starToColor = document.getElementById(j);
                starToColor.innerHTML = '&#9733;';
            }

            for (let j = 5; j > star.id; j--) {
                const starToColor = document.getElementById(j);
                starToColor.innerHTML = '&#9734;';
            }

            reviewInput.value = star.id;
        });
    });
}

