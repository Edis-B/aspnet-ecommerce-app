document.addEventListener('DOMContentLoaded', () => {
    appendPopupContainer();

    addToCart();
    addFavoriting();

    categoriesMenu();
    addImageStockBlocker();
});

async function categoriesMenu() {
    const categories = document.querySelectorAll('#category-filter-div ul li a');
    const model = getModel();

    categories.forEach(optionA => {
        const currentCategory = model.category;
        const currentQuery = model.query ? model.query : '';

        if (currentCategory && optionA.id == currentCategory) {
            optionA.style.backgroundColor = 'white';
        }

        optionA.addEventListener('click', async () => {
            const newCategory = optionA.id;
            window.location.href = `/Search/Search?Category=${newCategory}&Query=${currentQuery}`;
        });

    });
}