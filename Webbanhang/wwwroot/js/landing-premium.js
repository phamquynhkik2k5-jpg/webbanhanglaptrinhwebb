// ============================================
// Particles Generation
// ============================================
function generateParticles() {
    const particlesContainer = document.getElementById('particles');
    const particleCount = 30;

    for (let i = 0; i < particleCount; i++) {
        const particle = document.createElement('div');
        particle.className = 'particle';
        
        const size = Math.random() * 4 + 1;
        const xPos = Math.random() * 100;
        const duration = Math.random() * 10 + 15;
        const delay = Math.random() * 5;

        particle.style.width = size + 'px';
        particle.style.height = size + 'px';
        particle.style.left = xPos + '%';
        particle.style.animationDuration = duration + 's';
        particle.style.animationDelay = delay + 's';

        particlesContainer.appendChild(particle);
    }
}

// ============================================
// Scroll Parallax & Animation Observer
// ============================================
function observeElements() {
    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.style.opacity = '1';
                entry.target.style.transform = 'translateY(0)';
            }
        });
    }, {
        threshold: 0.1,
        rootMargin: '0px 0px -100px 0px'
    });

    // Observe all menu cards
    document.querySelectorAll('.menu-card').forEach(card => {
        observer.observe(card);
    });

    // Observe form groups
    document.querySelectorAll('.form-group').forEach(group => {
        observer.observe(group);
    });

    // Observe section titles
    document.querySelectorAll('.section-title').forEach(title => {
        observer.observe(title);
    });
}

// ============================================
// Parallax Scrolling Effect
// ============================================
window.addEventListener('scroll', () => {
    const scrolled = window.pageYOffset;
    const parallax = document.querySelector('.background-3d');
    
    if (parallax) {
        parallax.style.transform = `translateY(${scrolled * 0.5}px)`;
    }

    // Smooth scroll animation for sections
    const sections = document.querySelectorAll('.section');
    sections.forEach(section => {
        const sectionTop = section.getBoundingClientRect().top;
        const windowHeight = window.innerHeight;
        
        if (sectionTop < windowHeight) {
            const progress = 1 - sectionTop / windowHeight;
            section.style.opacity = Math.min(1, 0.5 + progress * 0.5);
        }
    });
});

// ============================================
// Category Filter (Dynamic Products)
// ============================================
function initCategoryFilter() {
    const categoryTabs = document.querySelectorAll('.category-tab');
    const menuCards = document.querySelectorAll('.menu-card');

    categoryTabs.forEach(tab => {
        tab.addEventListener('click', () => {
            // Remove active class from all tabs
            categoryTabs.forEach(t => t.classList.remove('active'));
            // Add active class to clicked tab
            tab.classList.add('active');

            const categoryId = tab.getAttribute('data-category');

            // Filter cards with animation
            menuCards.forEach(card => {
                card.style.transition = 'all 0.3s ease';
                
                if (categoryId === 'all') {
                    card.style.display = 'block';
                    card.style.opacity = '1';
                } else {
                    const cardCategory = card.getAttribute('data-category');
                    if (cardCategory === categoryId) {
                        card.style.display = 'block';
                        card.style.opacity = '1';
                    } else {
                        card.style.opacity = '0';
                        card.style.display = 'none';
                    }
                }
            });
        });
    });

    // Set "All" as active by default
    if (categoryTabs.length > 0) {
        categoryTabs[0].classList.add('active');
    }
}

// ============================================
// Form Submission Handler
// ============================================
function initFormHandler() {
    const bookingForm = document.getElementById('bookingForm');
    
    if (bookingForm) {
        bookingForm.addEventListener('submit', async function(e) {
            e.preventDefault();
            
            // Get form values
            const name = document.getElementById('name').value;
            const email = document.getElementById('email').value;
            const date = document.getElementById('date').value;
            const service = document.getElementById('service').value;
            const message = document.getElementById('message').value;

            // Prepare payload
            const payload = {
                name: name,
                email: email,
                bookingDate: date,
                serviceType: service,
                notes: message
            };

            try {
                // Send to backend API
                const response = await fetch('/api/landing/book', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(payload)
                });

                const result = await response.json();

                if (result.success) {
                    // Show success message with animation
                    showNotification('success', `Cảm ơn ${name}! Chúng tôi sẽ liên hệ với bạn tại ${email}`);
                    
                    // Reset form
                    bookingForm.reset();
                    
                    // Optional: Redirect after 2 seconds
                    setTimeout(() => {
                        // window.location.href = '/';
                    }, 2000);
                } else {
                    showNotification('error', result.message || 'Đã xảy ra lỗi. Vui lòng thử lại.');
                }
            } catch (error) {
                console.error('Error:', error);
                showNotification('error', 'Lỗi kết nối. Vui lòng thử lại sau.');
            }
        });
    }
}

// ============================================
// Notification System
// ============================================
function showNotification(type, message) {
    // Create notification element
    const notification = document.createElement('div');
    notification.className = `notification notification-${type}`;
    notification.innerHTML = `
        <div class="notification-content">
            <i class="fas fa-${type === 'success' ? 'check-circle' : 'exclamation-circle'}"></i>
            <p>${message}</p>
        </div>
    `;

    // Style notification
    Object.assign(notification.style, {
        position: 'fixed',
        top: '20px',
        right: '20px',
        padding: '1.5rem 2rem',
        borderRadius: '4px',
        zIndex: '10000',
        minWidth: '300px',
        animation: 'slideInRight 0.4s ease-out',
        backgroundColor: type === 'success' ? 'rgba(76, 175, 80, 0.9)' : 'rgba(244, 67, 54, 0.9)',
        color: 'white',
        boxShadow: '0 4px 12px rgba(0,0,0,0.15)',
        fontWeight: '500'
    });

    document.body.appendChild(notification);

    // Remove notification after 3 seconds
    setTimeout(() => {
        notification.style.animation = 'slideOutRight 0.4s ease-out';
        setTimeout(() => {
            notification.remove();
        }, 400);
    }, 3000);
}

// ============================================
// Smooth Scroll Navigation
// ============================================
function initSmoothScroll() {
    document.querySelectorAll('.nav-item a').forEach(link => {
        link.addEventListener('click', function(e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({ behavior: 'smooth', block: 'start' });
            }
        });
    });
}

// ============================================
// Add Slide Animations to CSS
// ============================================
function addAnimationStyles() {
    const style = document.createElement('style');
    style.textContent = `
        @keyframes slideInRight {
            from {
                transform: translateX(400px);
                opacity: 0;
            }
            to {
                transform: translateX(0);
                opacity: 1;
            }
        }

        @keyframes slideOutRight {
            from {
                transform: translateX(0);
                opacity: 1;
            }
            to {
                transform: translateX(400px);
                opacity: 0;
            }
        }

        .notification-content {
            display: flex;
            align-items: center;
            gap: 1rem;
        }

        .notification-content i {
            font-size: 1.5rem;
        }

        .notification-content p {
            margin: 0;
        }
    `;
    document.head.appendChild(style);
}

// ============================================
// Scroll to Top Button
// ============================================
function initScrollToTop() {
    const scrollBtn = document.createElement('button');
    scrollBtn.id = 'scrollToTop';
    scrollBtn.innerHTML = '<i class="fas fa-arrow-up"></i>';
    scrollBtn.style.cssText = `
        position: fixed;
        bottom: 30px;
        right: 30px;
        width: 50px;
        height: 50px;
        border-radius: 50%;
        background: linear-gradient(135deg, var(--warm-gold) 0%, var(--soft-gold) 100%);
        color: #0a0a0a;
        border: none;
        cursor: pointer;
        display: none;
        align-items: center;
        justify-content: center;
        z-index: 999;
        transition: all 0.3s ease;
        font-size: 1.2rem;
        box-shadow: 0 4px 12px rgba(212, 175, 55, 0.3);
    `;

    document.body.appendChild(scrollBtn);

    window.addEventListener('scroll', () => {
        if (window.pageYOffset > 300) {
            scrollBtn.style.display = 'flex';
        } else {
            scrollBtn.style.display = 'none';
        }
    });

    scrollBtn.addEventListener('click', () => {
        window.scrollTo({ top: 0, behavior: 'smooth' });
    });

    scrollBtn.addEventListener('mouseover', () => {
        scrollBtn.style.transform = 'translateY(-5px)';
    });

    scrollBtn.addEventListener('mouseout', () => {
        scrollBtn.style.transform = 'translateY(0)';
    });
}

// ============================================
// Enhanced Hover Effects
// ============================================
function enhanceHoverEffects() {
    const buttons = document.querySelectorAll('.cta-button, .submit-button');
    
    buttons.forEach(button => {
        button.addEventListener('mouseenter', function() {
            this.style.transform = 'translateY(-3px)';
        });

        button.addEventListener('mouseleave', function() {
            this.style.transform = 'translateY(0)';
        });
    });
}

// ============================================
// Initialize All Functions
// ============================================
window.addEventListener('load', () => {
    // Add animation styles first
    addAnimationStyles();

    // Generate particles
    generateParticles();

    // Initialize observers
    observeElements();

    // Initialize category filter
    initCategoryFilter();

    // Initialize form handler
    initFormHandler();

    // Initialize smooth scroll
    initSmoothScroll();

    // Initialize scroll to top button
    initScrollToTop();

    // Enhance hover effects
    enhanceHoverEffects();

    console.log('🎨 Sakura Premium Landing Page initialized successfully!');
});

// ============================================
// Utility Functions
// ============================================

// Format currency (Vietnamese Dong)
function formatCurrency(amount) {
    return new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND'
    }).format(amount);
}

// Check if element is in viewport
function isInViewport(element) {
    const rect = element.getBoundingClientRect();
    return (
        rect.top >= 0 &&
        rect.left >= 0 &&
        rect.bottom <= (window.innerHeight || document.documentElement.clientHeight) &&
        rect.right <= (window.innerWidth || document.documentElement.clientWidth)
    );
}

// Debounce function for scroll events
function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

// ============================================
// Mobile Menu Toggle (Optional)
// ============================================
function initMobileMenu() {
    const navMenu = document.querySelector('.nav-menu');
    const logo = document.querySelector('.logo');

    // Add mobile menu button if needed
    if (window.innerWidth <= 768) {
        const menuBtn = document.createElement('button');
        menuBtn.className = 'mobile-menu-btn';
        menuBtn.innerHTML = '<i class="fas fa-bars"></i>';
        menuBtn.style.cssText = `
            display: none;
            background: none;
            border: none;
            color: var(--warm-gold);
            font-size: 1.5rem;
            cursor: pointer;
            z-index: 1001;
        `;

        const nav = document.querySelector('nav');
        nav.appendChild(menuBtn);
    }
}

// Initialize on page load
document.addEventListener('DOMContentLoaded', () => {
    initMobileMenu();
});
