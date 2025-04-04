// Toggle sidebar
const sidebar = document.getElementById('sidebar');
const mainContent = document.getElementById('main-content');
const toggleBtn = document.getElementById('toggle-sidebar');
const themeToggle = document.getElementById('theme-toggle');
const body = document.body;
const submenuToggles = document.querySelectorAll('.submenu-toggle');
const logo = document.getElementById("logo");

// Toggle sidebar collapse
toggleBtn.addEventListener('click', () => {
    sidebar.classList.toggle('collapsed');
    mainContent.classList.toggle('expanded');

    // For mobile
    sidebar.classList.toggle('mobile-open');
    mainContent.classList.toggle('mobile-expanded');
});

// Toggle submenus
submenuToggles.forEach(toggle => {
    toggle.addEventListener('click', (e) => {
        e.preventDefault();
        const submenu = toggle.nextElementSibling;
        submenu.classList.toggle('open');

        // Toggle arrow direction
        const arrow = toggle.querySelector('.arrow');
        if (arrow) {
            arrow.style.transform = submenu.classList.contains('open') ? 'rotate(180deg)' : 'rotate(0)';
        }
    });
});

// Toggle theme
themeToggle.addEventListener('click', (e) => {
    e.preventDefault();
    const currentTheme = body.getAttribute('data-theme') || 'light';
    const newTheme = currentTheme === 'light' ? 'dark' : 'light';
    body.setAttribute('data-theme', newTheme);

    // Update theme icon
    const themeIcon = themeToggle.querySelector('.nav-icon');
    themeIcon.className = newTheme === 'light' ? 'fa-solid fa-moon nav-icon' : 'fa-solid fa-sun nav-icon';
});



