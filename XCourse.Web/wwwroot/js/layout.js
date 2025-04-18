// Toggle sidebar
const sidebar = document.getElementById('sidebar');
const mainContent = document.getElementById('main-content');
const toggleBtn = document.getElementById('toggle-sidebar');
const themeToggle = document.getElementById('theme-toggle');
const body = document.body;
const submenuToggles = document.querySelectorAll('.submenu-toggle');
const logo = document.getElementById("logo");
const themeLight = document.getElementById('theme-light');
const themeDark = document.getElementById('theme-dark');
const themeSystem = document.getElementById('theme-system');
const modeBar = document.getElementById('modebar');
const favicon = document.getElementById('favicon');
const logoText = document.getElementById('logo-text');

// Toggle sidebar collapse
toggleBtn.addEventListener('click', () => {
    sidebar.classList.toggle('collapsed');
    mainContent.classList.toggle('expanded');
    modeBar.classList.toggle('expanded')
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
    modeBar.classList.toggle('d-none');
});

function applyTheme(theme) {
    document.documentElement.setAttribute('data-theme', theme);
    localStorage.setItem('theme', theme);
    modeBar.classList.add('d-none');
    favicon.href = `/favicons/favicon-${theme}.svg`;
    logoText.src = `/branding/logo-en-${theme}.svg`;
}

function applyLightTheme() {
    applyTheme('light');
    localStorage.setItem('mode', 'user');
    themeToggle.innerHTML = `<i class="fa-solid fa-sun nav-icon"></i>
                                <span class="nav-text">Toggle Theme</span>`
}

function applyDarkTheme() {
    applyTheme('dark');
    localStorage.setItem('mode', 'user');
    themeToggle.innerHTML = `<i class="fa-solid fa-moon nav-icon"></i>
                                <span class="nav-text">Toggle Theme</span>`
}

function applySystemTheme() {
    if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
        applyTheme('dark');
    } else {
        applyTheme('light');
    }
    localStorage.setItem('mode', 'system');
    themeToggle.innerHTML = `<i class="fa-solid fa-laptop nav-icon"></i>
                                <span class="nav-text">Toggle Theme</span>`
}

themeLight.addEventListener('click', applyLightTheme);

themeDark.addEventListener('click', applyDarkTheme);

themeSystem.addEventListener('click', applySystemTheme);

onload = () => {
    const mode = localStorage.getItem('mode');
    if (mode) {
        if (mode == 'system') {
            applySystemTheme();
        } else {
            const theme = localStorage.getItem('theme');
            if (theme) {
                if (theme == 'dark') {
                    applyDarkTheme();
                }
            } else {
                applyLightTheme();
            }
        }
    } else {
        applyLightTheme();
    }
}


// Toast counter to generate unique IDs
let toastCounter = 0;

// Function to show success toast
function showSuccessToast(title, message, duration) {
    // If duration is not provided, get it from the input field
    if (duration === undefined) {
        duration = parseInt(document.getElementById('duration').value, 10) || 5;
    }

    const checkmarkSvg = `
        <svg width="30" height="30" viewBox="0 0 52 52" xmlns="http://www.w3.org/2000/svg">
          <circle class="checkmark-circle" cx="26" cy="26" r="25" fill="none" stroke="currentColor" stroke-width="2" />
          <path class="checkmark" fill="none" stroke="currentColor" stroke-width="3" d="M14.1 27.2l7.1 7.2 16.7-16.8" />
        </svg>
      `;

    createToast(title, message, 'success', checkmarkSvg, duration);
}

// Function to show error toast
function showErrorToast(title, message, duration) {
    // If duration is not provided, get it from the input field
    if (duration === undefined) {
        duration = parseInt(document.getElementById('duration').value, 10) || 5;
    }

    const xmarkSvg = `
        <svg width="30" height="30" viewBox="0 0 52 52" xmlns="http://www.w3.org/2000/svg">
          <circle cx="26" cy="26" r="25" fill="none" stroke="currentColor" stroke-width="2" />
          <path class="xmark" fill="none" stroke="currentColor" stroke-width="3" d="M16 16 36 36 M36 16 16 36" />
        </svg>
      `;

    createToast(title, message, 'error', xmarkSvg, duration);
}

// Function to create a toast notification
function createToast(title, message, type, iconSvg, duration) {
    const toastId = `toast-${toastCounter++}`;
    let toastContainer = document.getElementById('toast-container');

    if (toastContainer == null || toastContainer == undefined) {
        const div = document.createElement('div');
        div.className = 'toast-container';
        div.id = 'toast-container';
        document.body.appendChild(div);

        toastContainer = document.getElementById('toast-container');
    }

    // Remove any existing toasts
    toastContainer.innerHTML = '';

    // Create toast element
    const toast = document.createElement('div');
    toast.id = toastId;
    toast.className = `toast ${type} zoom-in`;
    toast.onclick = () => dismissToast(toastId);

    // Set inner HTML with icon, title, and message
    toast.innerHTML = `
        <div class="toast-icon">${iconSvg}</div>
        <div class="toast-content">
          <div class="toast-title">${title}</div>
          <div class="toast-message">${message}</div>
        </div>
        <div class="toast-progress" style="animation: progress-shrink ${duration}s linear forwards;"></div>
      `;

    // Add toast to container
    toastContainer.appendChild(toast);

    // Add progress animation
    const progressBar = toast.querySelector('.toast-progress');
    progressBar.style.animation = `progress-shrink ${duration}s linear forwards`;

    // Auto dismiss after duration
    setTimeout(() => {
        dismissToast(toastId);
    }, duration * 1000);
}

// Function to dismiss a toast notification
function dismissToast(toastId) {
    const toast = document.getElementById(toastId);
    if (!toast) return;

    // Apply exit animation
    toast.classList.remove('zoom-in');
    toast.classList.add('zoom-out');

    // Remove toast after animation completes
    setTimeout(() => {
        if (toast && toast.parentNode) {
            toast.parentNode.removeChild(toast);
        }
    }, 400);
}

// Define progress-shrink animation here because it's dynamically set
document.head.insertAdjacentHTML('beforeend', `
      <style>
        @keyframes progress-shrink {
          from {
            width: 100%;
          }
          to {
            width: 0%;
          }
        }
      </style>
    `);