// ===============================================
// GÉNÉRATION AUTOMATIQUE DE MATRICULES IIT
// ===============================================

document.addEventListener('DOMContentLoaded', function() {
    const niveauSelect = document.getElementById('Niveau');
    const matriculePreview = document.querySelector('input[readonly]');
    
    if (niveauSelect && matriculePreview) {
        niveauSelect.addEventListener('change', function() {
            const niveau = this.value;
            const currentYear = new Date().getFullYear();
            
            if (niveau) {
                // Simuler le format du matricule qui sera généré
                matriculePreview.value = `IIT${currentYear}${niveau}XXX (sera généré automatiquement)`;
                matriculePreview.classList.remove('bg-light');
                matriculePreview.classList.add('bg-success', 'text-white');
                
                // Mettre à jour le texte d'aide
                const helpText = matriculePreview.parentElement.querySelector('small');
                if (helpText) {
                    helpText.innerHTML = `<i class="fas fa-check-circle"></i> Format: IIT${currentYear}${niveau}001, IIT${currentYear}${niveau}002, etc.`;
                    helpText.className = 'form-text text-success';
                }
            } else {
                matriculePreview.value = 'Veuillez sélectionner un niveau';
                matriculePreview.classList.remove('bg-success', 'text-white');
                matriculePreview.classList.add('bg-light');
                
                const helpText = matriculePreview.parentElement.querySelector('small');
                if (helpText) {
                    helpText.innerHTML = '<i class="fas fa-info-circle"></i> Génération automatique activée';
                    helpText.className = 'form-text text-muted';
                }
            }
        });
        
        // Animation au focus sur le niveau
        niveauSelect.addEventListener('focus', function() {
            this.parentElement.classList.add('animate__animated', 'animate__pulse');
            setTimeout(() => {
                this.parentElement.classList.remove('animate__animated', 'animate__pulse');
            }, 1000);
        });
    }
    
    // Validation côté client pour les matricules existants (si édition)
    const matriculeInputs = document.querySelectorAll('input[data-matricule]');
    matriculeInputs.forEach(input => {
        input.addEventListener('input', function() {
            const matricule = this.value;
            const pattern = /^IIT\d{4}(L[123]|M[12])\d{3}$/;
            
            if (matricule && !pattern.test(matricule)) {
                this.classList.add('is-invalid');
                this.classList.remove('is-valid');
                
                let feedback = this.parentElement.querySelector('.invalid-feedback');
                if (!feedback) {
                    feedback = document.createElement('div');
                    feedback.className = 'invalid-feedback';
                    this.parentElement.appendChild(feedback);
                }
                feedback.textContent = 'Format invalide. Attendu: IIT2025L1001, IIT2025L2001, etc.';
            } else if (matricule) {
                this.classList.remove('is-invalid');
                this.classList.add('is-valid');
                
                const feedback = this.parentElement.querySelector('.invalid-feedback');
                if (feedback) {
                    feedback.remove();
                }
            }
        });
    });
});

// Fonction utilitaire pour extraire des informations du matricule
function parseMatricule(matricule) {
    const match = matricule.match(/^IIT(\d{4})(L[123]|M[12])(\d{3})$/);
    if (match) {
        return {
            prefix: 'IIT',
            annee: match[1],
            niveau: match[2],
            numero: match[3],
            valid: true
        };
    }
    return { valid: false };
}

// Fonction pour afficher des statistiques de matricules (pour le dashboard)
function displayMatriculeStats(matricules) {
    const stats = {};
    matricules.forEach(matricule => {
        const parsed = parseMatricule(matricule);
        if (parsed.valid) {
            const key = `${parsed.annee}-${parsed.niveau}`;
            stats[key] = (stats[key] || 0) + 1;
        }
    });
    
    return stats;
}