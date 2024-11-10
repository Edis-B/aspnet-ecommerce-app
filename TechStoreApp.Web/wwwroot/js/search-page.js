document.addEventListener('DOMContentLoaded', () => {
    appendPopupContainer();

    addToCart();
    addFavoriting();

    categoriesMenu();
    addImageStockBlocker();
});

async function categoriesMenu() {
    const categories = document.querySelectorAll('#filter-by-category ul li button');
    const model = getModel();

    categories.forEach(button => {
        const currentCategory = model.category;

        if (currentCategory && button.value == currentCategory) {
            button.style.backgroundColor = 'gray';
        }
    });
}