document.addEventListener('DOMContentLoaded', function () {
    appendPopupContainer();

    addToCart();
    addFavoriting();

    starAnimations();
    addImageStockBlocker();
});

async function starAnimations() {
    const allStars = document.querySelectorAll('.single-star');

    allStars.forEach(star => {
        star.addEventListener('click', () => {
            const index = parseInt(star.id, 10); 

            for (let j = 1; j <= index; j++) {
                const starToChange = document.getElementById(j); 
                const smt = starToChange.querySelector('span'); 
                smt.innerHTML = '&#9733;';
                smt.style.color = 'orange'; 
            }

            for (let j = index + 1; j <= 5; j++) {
                const starToChange = document.getElementById(j); 
                const smt = starToChange.querySelector('span'); 
                smt.innerHTML = '&#9734;';
                smt.style.color = '#ccc'; 
            }
        });
    });

}

