document.addEventListener('DOMContentLoaded', function () {
    const registrationForm = document.querySelector('.registration-form');

    // Get input fields by their generated IDs (Django typically uses id_fieldname)
    const usernameField = document.getElementById('id_username');
    const firstNameField = document.getElementById('id_first_name');
    const lastNameField = document.getElementById('id_last_name');
    const emailField = document.getElementById('id_email');
    const passwordField = document.getElementById('id_password1'); // Django's UserCreationForm uses password1 and password2
    const confirmPasswordField = document.getElementById('id_password2');
    const rutField = document.getElementById('id_rut');
    const tipousuField = document.getElementById('id_tipousu'); // For select element
    const dirusuField = document.getElementById('id_dirusu');

    // --- Helper function to display error messages ---
    function displayError(field, message) {
        // Find existing error message or create a new one
        let errorElement = field.parentNode.querySelector('.client-validation-error-message');
        if (!errorElement) {
            errorElement = document.createElement('span');
            errorElement.classList.add('client-validation-error-message');
            errorElement.style.color = 'red';
            errorElement.style.fontSize = '0.85em';
            errorElement.style.marginTop = '5px';
            field.parentNode.appendChild(errorElement); // Append to parent of the field
        }
        errorElement.textContent = message;
        field.classList.add('is-invalid');
    }

    // --- Helper function to clear error messages ---
    function clearError(field) {
        let errorElement = field.parentNode.querySelector('.client-validation-error-message');
        if (errorElement) {
            errorElement.remove();
        }
        field.classList.remove('is-invalid');
    }

    // --- Password Visibility Toggle ---
    function createPasswordToggle(field) {
        if (!field) return;

        const wrapper = field.parentNode; // The <p> tag containing the input
        const toggleBtn = document.createElement('span');
        toggleBtn.classList.add('password-toggle');
        toggleBtn.innerHTML = '<i class="far fa-eye"></i>'; // Font Awesome eye icon
        toggleBtn.style.cursor = 'pointer';

        // Append toggle button after the input field within the same parent
        wrapper.insertBefore(toggleBtn, field.nextSibling);

        toggleBtn.addEventListener('click', function () {
            if (field.type === 'password') {
                field.type = 'text';
                toggleBtn.innerHTML = '<i class="far fa-eye-slash"></i>';
            } else {
                field.type = 'password';
                toggleBtn.innerHTML = '<i class="far fa-eye"></i>';
            }
        });
    }

    // Apply toggle to both password fields
    createPasswordToggle(passwordField);
    createPasswordToggle(confirmPasswordField);

    // --- Client-Side Validation on Form Submission ---
    if (registrationForm) {
        registrationForm.addEventListener('submit', function (event) {
            let isValid = true;

            // Clear all previous client-side errors before re-validating
            document.querySelectorAll('.client-validation-error-message').forEach(el => el.remove());
            document.querySelectorAll('.is-invalid').forEach(el => el.classList.remove('is-invalid'));

            // 1. Username Validation (basic check if empty)
            if (usernameField && usernameField.value.trim() === '') {
                displayError(usernameField, 'El nombre de usuario es obligatorio.');
                isValid = false;
            }

            // 2. First Name Validation
            if (firstNameField && firstNameField.value.trim() === '') {
                displayError(firstNameField, 'El nombre es obligatorio.');
                isValid = false;
            }

            // 3. Last Name Validation
            if (lastNameField && lastNameField.value.trim() === '') {
                displayError(lastNameField, 'El apellido es obligatorio.');
                isValid = false;
            }

            // 4. Email Validation (@ symbol and basic dot check)
            if (emailField) {
                const email = emailField.value.trim();
                if (email === '' || !email.includes('@') || !email.includes('.')) {
                    displayError(emailField, 'Por favor, ingresa una dirección de correo válida.');
                    isValid = false;
                }
            }

            // 5. Password Strength Validation
            if (passwordField) {
                const password = passwordField.value;
                if (password.length < 8) {
                    displayError(passwordField, 'La contraseña debe tener al menos 8 caracteres.');
                    isValid = false;
                } else if (!/[A-Z]/.test(password)) {
                    displayError(passwordField, 'La contraseña debe contener al menos una letra mayúscula.');
                    isValid = false;
                } else if (!/[a-z]/.test(password)) {
                    displayError(passwordField, 'La contraseña debe contener al menos una letra minúscula.');
                    isValid = false;
                } else if (!/[0-9]/.test(password)) {
                    displayError(passwordField, 'La contraseña debe contener al menos un número.');
                    isValid = false;
                } else if (!/[!@#$%^&*(),.?":{}|<>]/.test(password)) {
                    displayError(passwordField, 'La contraseña debe contener al menos un carácter especial.');
                    isValid = false;
                }
            }

            // 6. Password Confirmation Validation
            if (passwordField && confirmPasswordField) {
                if (passwordField.value !== confirmPasswordField.value) {
                    displayError(confirmPasswordField, 'Las contraseñas no coinciden.');
                    isValid = false;
                }
            }

            // 7. RUT Validation (Basic format check client-side, full algorithm server-side)
            if (rutField && rutField.value.trim() === '') {
                displayError(rutField, 'El RUT es obligatorio.');
                isValid = false;
            } else if (rutField && !/^\d{1,2}\.\d{3}\.\d{3}[-][0-9Kk]$/.test(rutField.value.trim()) && !/^\d+[-][0-9Kk]$/.test(rutField.value.trim())) {
                displayError(rutField, 'Formato de RUT inválido. Use 12.345.678-9 o 12345678-9.');
                isValid = false;
            }

            // 8. Tipo de Usuario Validation (should be handled by Django's ChoiceField, but good to have)
            if (tipousuField && tipousuField.value.trim() === '') {
                displayError(tipousuField, 'El tipo de usuario es obligatorio.');
                isValid = false;
            }

            // 9. Dirección Validation
            if (dirusuField && dirusuField.value.trim() === '') {
                displayError(dirusuField, 'La dirección es obligatoria.');
                isValid = false;
            }


            if (!isValid) {
                event.preventDefault(); // Prevent form submission if validation fails
            }
        });

        // --- Real-time Validation (on 'input' and 'blur' events) ---

        // Generic real-time validation for text inputs (username, first_name, last_name, dirusu)
        [usernameField, firstNameField, lastNameField, dirusuField].forEach(field => {
            if (field) {
                field.addEventListener('input', function () {
                    if (field.value.trim() === '') {
                        displayError(field, `El campo '${field.labels[0].textContent.replace(':', '')}' es obligatorio.`);
                    } else {
                        clearError(field);
                    }
                });
            }
        });

        // Real-time email validation
        if (emailField) {
            emailField.addEventListener('input', function () {
                const email = emailField.value.trim();
                if (email === '' || !email.includes('@') || !email.includes('.')) {
                    displayError(emailField, 'Por favor, ingresa una dirección de correo válida.');
                } else {
                    clearError(emailField);
                }
            });
        }

        // Real-time password strength validation
        if (passwordField) {
            passwordField.addEventListener('input', function () {
                const password = passwordField.value;
                let errorMessage = '';
                if (password.length < 8) {
                    errorMessage = 'Debe tener al menos 8 caracteres.';
                } else if (!/[A-Z]/.test(password)) {
                    errorMessage = 'Debe contener una mayúscula.';
                } else if (!/[a-z]/.test(password)) {
                    errorMessage = 'Debe contener una minúscula.';
                } else if (!/[0-9]/.test(password)) {
                    errorMessage = 'Debe contener un número.';
                } else if (!/[!@#$%^&*(),.?":{}|<>]/.test(password)) {
                    errorMessage = 'Debe contener un carácter especial.';
                }

                if (errorMessage) {
                    displayError(passwordField, `Contraseña: ${errorMessage}`);
                } else {
                    clearError(passwordField);
                }
            });
        }

        // Real-time password confirmation validation
        if (confirmPasswordField) {
            confirmPasswordField.addEventListener('input', function () {
                if (passwordField && passwordField.value !== confirmPasswordField.value) {
                    displayError(confirmPasswordField, 'Las contraseñas no coinciden.');
                } else {
                    clearError(confirmPasswordField);
                }
            });
        }

        // Real-time RUT validation (basic format)
        if (rutField) {
            rutField.addEventListener('input', function () {
                const rut = rutField.value.trim();
                if (rut === '') {
                    displayError(rutField, 'El RUT es obligatorio.');
                } else if (!/^\d{1,2}\.\d{3}\.\d{3}[-][0-9Kk]$/.test(rut) && !/^\d+[-][0-9Kk]$/.test(rut)) {
                    displayError(rutField, 'Formato de RUT inválido. Use 12.345.678-9 o 12345678-9.');
                } else {
                    clearError(rutField);
                }
            });
        }

        // Real-time tipousu validation (for select, usually valid unless empty)
        if (tipousuField) {
            tipousuField.addEventListener('change', function () {
                if (tipousuField.value.trim() === '') {
                    displayError(tipousuField, 'El tipo de usuario es obligatorio.');
                } else {
                    clearError(tipousuField);
                }
            });
        }
    }

    // --- Login Link Redirection ---
    const loginLink = document.querySelector('.login-link');
    if (loginLink) {
        loginLink.addEventListener('click', function (event) {
            event.preventDefault();
            window.location.href = '{% url "iniciar_sesion" %}'; // Corrected URL name
        });
    }
});