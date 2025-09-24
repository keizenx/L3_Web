// Custom JavaScript for IIT University Management System

// Auto-refresh dashboard statistics
function refreshDashboardStats() {
    // This function can be used to refresh dashboard statistics via AJAX
    console.log('Dashboard statistics refreshed');
}

// Initialize DataTables with French language
function initializeDataTables() {
    if (typeof $.fn.DataTable !== 'undefined') {
        $('.dataTable').DataTable({
            "language": {
                "url": "//cdn.datatables.net/plug-ins/1.10.24/i18n/French.json"
            },
            "pageLength": 25,
            "responsive": true
        });
    }
}

// Form validation enhancement
function enhanceFormValidation() {
    // Add custom validation rules if needed
    $('form').on('submit', function() {
        var isValid = true;
        
        // Check required fields
        $(this).find('[required]').each(function() {
            if (!$(this).val()) {
                $(this).addClass('is-invalid');
                isValid = false;
            } else {
                $(this).removeClass('is-invalid');
            }
        });
        
        return isValid;
    });
}

// Initialize all components when document is ready
$(document).ready(function() {
    console.log('IIT University Management System initialized');
    
    // Initialize DataTables
    initializeDataTables();
    
    // Enhance form validation
    enhanceFormValidation();
    
    // Auto-refresh dashboard every 30 seconds if on dashboard page
    if (window.location.pathname === '/') {
        setInterval(function() {
            refreshDashboardStats();
        }, 30000);
    }
});
