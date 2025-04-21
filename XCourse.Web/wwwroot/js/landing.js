const themeToggle = document.getElementById('mode-button');
const favicon = document.getElementById('favicon');
const logoText = document.getElementById('logo-text');
const logoTextAr = document.getElementById('logo-text-ar');
const logoTextFoot = document.getElementById('logo-text-foot');
const modeBar = document.getElementById('mode-bar');
const themeLight = document.getElementById('theme-light');
const themeDark = document.getElementById('theme-dark');
const themeSystem = document.getElementById('theme-system');


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
    logoTextFoot.src = `/branding/logo-en-${theme}.svg`;
    logoTextAr.src = `/branding/logo-ar-${theme}.svg`;
}

function applyLightTheme() {
    applyTheme('light');
    localStorage.setItem('mode', 'user');
    themeToggle.innerHTML = `<i class="fa-solid fa-sun"></i>`
}

function applyDarkTheme() {
    applyTheme('dark');
    localStorage.setItem('mode', 'user');
    themeToggle.innerHTML = `<i class="fa-solid fa-moon"></i>`
}

function applySystemTheme() {
    if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
        applyTheme('dark');
    } else {
        applyTheme('light');
    }
    localStorage.setItem('mode', 'system');
    themeToggle.innerHTML = `<i class="fa-solid fa-laptop"></i>`
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


document.addEventListener('DOMContentLoaded', function () {
    // Set copyright year
    document.getElementById('copyright-year').textContent = new Date().getFullYear();

    // Mobile menu toggle
    const menuToggle = document.querySelector('.navbar__toggle');
    const navbarLinks = document.querySelector('.navbar__links');
    const navbarButtons = document.querySelector('.navbar__buttons');

    if (menuToggle) {
        menuToggle.addEventListener('click', function () {
            this.classList.toggle('navbar__toggle--active');

            // Create mobile menu if it doesn't exist
            if (!document.querySelector('.mobile-menu')) {
                const mobileMenu = document.createElement('div');
                mobileMenu.className = 'mobile-menu';

                // Clone navigation links and buttons
                const navClone = navbarLinks.cloneNode(true);
                const buttonsClone = navbarButtons.cloneNode(true);

                mobileMenu.appendChild(navClone);
                mobileMenu.appendChild(buttonsClone);

                // Insert after navbar
                const navbar = document.querySelector('.navbar');
                navbar.parentNode.insertBefore(mobileMenu, navbar.nextSibling);

                // Add styles
                Object.assign(mobileMenu.style, {
                    position: 'fixed',
                    top: navbar.offsetHeight + 'px',
                    left: '0',
                    right: '0',
                    backgroundColor: 'var(--clr-surface)',
                    padding: '1rem',
                    zIndex: '999',
                    display: 'none',
                    flexDirection: 'column',
                    gap: '1rem',
                    boxShadow: '0 4px 6px rgba(0, 0, 0, 0.1)'
                });

                // Style the cloned elements
                Object.assign(navClone.style, {
                    display: 'flex',
                    flexDirection: 'column',
                    gap: '1rem'
                });

                Object.assign(buttonsClone.style, {
                    display: 'flex',
                    flexDirection: 'column',
                    gap: '1rem'
                });

                const buttonClones = buttonsClone.querySelectorAll('.button');
                buttonClones.forEach(button => {
                    button.style.width = '100%';
                    button.style.marginLeft = '0';
                });
            }

            // Toggle mobile menu
            const mobileMenu = document.querySelector('.mobile-menu');
            if (mobileMenu) {
                if (mobileMenu.style.display === 'flex') {
                    mobileMenu.style.display = 'none';
                } else {
                    mobileMenu.style.display = 'flex';
                }
            }
        });
    }

    // Role showcase rotation
    const roleCards = document.querySelectorAll('.role-card');
    let currentRoleIndex = 0;

    if (roleCards.length) {
        function showNextRole() {
            roleCards[currentRoleIndex].classList.remove('role-card--active');
            currentRoleIndex = (currentRoleIndex + 1) % roleCards.length;
            roleCards[currentRoleIndex].classList.add('role-card--active');
        }

        setInterval(showNextRole, 5000);
    }

    // Animate testimonial cards on scroll with stagger and delay
    const testimonialCards = document.querySelectorAll('[data-aos]');
    let scrollTimeout = null;

    function animateOnScroll() {
        testimonialCards.forEach((card, index) => {
            const cardTop = card.getBoundingClientRect().top;
            const windowHeight = window.innerHeight;

            if (cardTop < windowHeight - 100) {
                // Apply delay using data-aos-delay or stagger by index
                const baseDelay = card.hasAttribute('data-aos-delay')
                    ? parseInt(card.getAttribute('data-aos-delay'))
                    : index * 100;

                card.style.transitionDelay = `${baseDelay}ms`;
                card.classList.add('aos-animate');
            }
        });
    }

    // Run on page load and scroll, with debounce
    window.addEventListener('scroll', () => {
        clearTimeout(scrollTimeout);
        scrollTimeout = setTimeout(animateOnScroll, 50);
    });

    animateOnScroll();
});

window.addEventListener("scroll", function () {
    const navbar = document.getElementById("navbar");
    const triggerElement = document.getElementById("hero");
  
    const triggerPosition = triggerElement.getBoundingClientRect().bottom;
  
    if (triggerPosition < 0) {
      navbar.classList.remove("hidden");
    } else {
      navbar.classList.add("hidden");
    }
  });
