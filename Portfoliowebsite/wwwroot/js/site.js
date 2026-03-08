//Email validatie
function isValidEmail(email) {
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
}


function setupValidation() {
    const form = document.getElementById('contactForm');
    const hp = document.getElementById('website');
    const email = document.getElementById('Email');
    const name = document.getElementById('Name');
    const msg = document.getElementById('Message');
    const status = document.getElementById('liveStatus');


    const showError = (id, message) => {
        document.getElementById(id).textContent = message;  // door textContent te gebruiken, voorkom je dat er HTML geïnjecteerd kan worden
    };

    //ververst de errors bij elke submit, zodat er geen oude errors blijven staan
    const clearError = (id) => {
        document.getElementById(id).textContent = '';
    };

   
    form.addEventListener('submit', (e) => {

        let hasErrors = false;
        //error clearen
        clearError('nameErr');
        clearError('emailErr');
        clearError('subjectErr');
        clearError('msgErr');

        // Honeypot check
        if (hp.value) {
            e.preventDefault();
            return false;
        }

        // Name validatie
        if (!name.value.trim() || name.value.length < 2) {
            showError('nameErr', 'Naam moet minimaal 2 tekens zijn');
            hasErrors = true;
        }

        // Email validatie
        if (!email.value.trim() || !isValidEmail(email.value)) {
            showError('emailErr', 'Voer een geldig e-mailadres in');
            hasErrors = true;
        }

        // Message validatie
        if (!msg.value.trim() || msg.value.length < 10) {
            showError('msgErr', 'Bericht moet minimaal 10 tekens zijn');
            hasErrors = true;
        }

        if (hasErrors) {
            e.preventDefault();
            status.textContent = 'Corrigeer de fouten';
            return false;
        }

        return true;
    });
}

window.addEventListener('DOMContentLoaded', setupValidation);