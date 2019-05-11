const toggleButton = gei("menu-toggle");
const wrapper = gei("wrapper");

function toggleMenu(e) {
    e.preventDefault();
    $(wrapper).toggleClass("toggled");
}

toggleButton.onclick = toggleMenu;
