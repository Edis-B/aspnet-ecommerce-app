document.addEventListener('DOMContentLoaded', function () {
    appendPopupContainer();

    addToCart();
    addFavoriting();

    starAnimations();
    postReview();
    addImageStockBlocker();
});

async function postReview() {
    const allStars = document.querySelectorAll('.single-star');

    const postReviewForm = document.getElementById('post-review-form');

    postReviewForm.addEventListener('submit', async (event) => {
        event.preventDefault();

        validators = postReviewForm.querySelectorAll('span');
        validators.forEach(span => {
            span.textContent = ''
        });

        // postReviewForm.elements['productId'].value
        // postReviewForm.elements['comment'].value
        // postReviewForm.elements['rating'].value

        const productIdInput = document.getElementById('product-id-input');
        const commentInput = document.getElementById('review-comment-input');
        const ratingInput = document.getElementById('userRating');

        const productId = productIdInput.value;
        const comment = commentInput.value;
        const rating = ratingInput.value;

        // Validate Fields
        let check = true;
        if (!comment || comment.length < 10) {
            displayValidationError('Comment must be at least 10 characters long.', 'Comment');
            check = false;
        }

        if (!rating) {
            displayValidationError('Please select rating!', 'Rating');
            check = false;
        }

        if (!check) {
            return;
        }

        const response = await fetch('/Product/CreateReview', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                ProductId: productId,
                Comment: comment,
                Rating: rating
            })
        })

        if (response.ok) {
            commentvalue = '';
            ratingvalue = '';

            allStars.forEach(el => {
                el.innerHTML = '&#9734;';
            });
        }
    })
}
function displayValidationError(message, field) {
    let el = document.getElementById(`validate-${field}`);
    el.textContent = message;
}

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

