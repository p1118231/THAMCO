// Initialize Google Places Autocomplete for Payment Address
function initializeAutocomplete() {
    const paymentAddressInput = document.getElementById("PaymentAddress");
    if (paymentAddressInput) {
        const autocomplete = new google.maps.places.Autocomplete(paymentAddressInput);
        autocomplete.setFields(["address_components", "geometry", "formatted_address"]);
        autocomplete.addListener("place_changed", function () {
            const place = autocomplete.getPlace();
            console.log("Selected address:", place.formatted_address);
        });
    }
}

google.maps.event.addDomListener(window, "load", initializeAutocomplete);

// Initialize intl-tel-input for Phone Number
function initializeIntlTelInput() {
    const phoneNumberInput = document.querySelector("#phoneNumber");
    if (phoneNumberInput) {
        const intlInput = window.intlTelInput(phoneNumberInput, {
            initialCountry: "auto",
            geoIpLookup: function (callback) {
                fetch("https://ipinfo.io/json?token=YOUR_IPINFO_TOKEN")
                    .then((resp) => resp.json())
                    .then((data) => callback(data.country))
                    .catch(() => callback("us"));
            },
            nationalMode:false,
            utilsScript: "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
        });

        document.querySelector("form").addEventListener("submit", function (e) {
            const fullPhoneNumber = intlInput.getNumber();
            if (!intlInput.isValidNumber()) {
                e.preventDefault();
                alert("Please enter a valid phone number.");
            } else {
                const hiddenInput = document.createElement("input");
                hiddenInput.type = "hidden";
                hiddenInput.name = "newValue";
                hiddenInput.value = fullPhoneNumber;
                this.appendChild(hiddenInput);
            }
        });
    }
}

initializeIntlTelInput();

document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");
    const passwordInput = document.getElementById("Password");
    const passwordError = document.getElementById("PasswordError");

    if (passwordInput) {
        form.addEventListener("submit", function (e) {
            // Regular expression for password validation
            const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;

            // Clear any previous error
            passwordError.style.display = "none";

            if (!passwordRegex.test(passwordInput.value)) {
                e.preventDefault(); // Prevent form submission
                passwordError.style.display = "block";
                passwordError.textContent =
                    "Password must have at least one uppercase letter, one lowercase letter, one number, one special character, and must be at least 8 characters long.";
            }
        });
    }

    // Toggle Password Visibility
    const togglePasswordButton = document.getElementById("togglePassword");
    if (togglePasswordButton) {
        togglePasswordButton.addEventListener("click", function () {
            const type =
                passwordInput.getAttribute("type") === "password" ? "text" : "password";
            passwordInput.setAttribute("type", type);

            // Toggle the eye icon
            this.querySelector("i").classList.toggle("bi-eye");
            this.querySelector("i").classList.toggle("bi-eye-slash");
        });
    }
});


document.querySelector("form").addEventListener("submit", function (e) {
    let isValid = true;

    // Check if fields are empty
    document.querySelectorAll("input[required]").forEach(function (input) {
        const errorSpan = input.nextElementSibling;
        if (!input.value.trim()) {
            isValid = false;
            errorSpan.style.display = "block";
            errorSpan.textContent = `${input.placeholder} is required.`;
        } else {
            errorSpan.style.display = "none";
        }
    });

    if (!isValid) {
        e.preventDefault(); // Prevent form submission
    }
});

//validate the email address
document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");
    const emailInput = document.getElementById("email");
    const emailError = document.getElementById("emailError");

    if (emailInput) {
        form.addEventListener("submit", function (e) {

            // Regular expression for email validation
            const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

            // Clear any previous error
            emailError.style.display = "none";

            if (!emailRegex.test(emailInput.value)) {
                e.preventDefault(); // Prevent form submission
                emailError.style.display = "block";
                emailError.textContent = "Please enter a valid email address.";
            }
        });
    }
});

//validation for phone number

document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");
    const phoneNumberInput = document.getElementById("phoneNumber");
    const phoneNumberError = phoneNumberInput
        ? phoneNumberInput.nextElementSibling // Assuming the error span is next to the input
        : null;

    if (phoneNumberInput) {
        form.addEventListener("submit", function (e) {
            // Regular expression for E.164 phone number format
            const phoneRegex = /^\+[1-9]\d{1,14}$/;

            // Clear any previous error
            if (phoneNumberError) {
                phoneNumberError.style.display = "none";
            }

            // Validate phone number
            if (!phoneRegex.test(phoneNumberInput.value)) {
                e.preventDefault(); // Prevent form submission
                if (phoneNumberError) {
                    phoneNumberError.style.display = "block";
                    phoneNumberError.textContent =
                        "Please enter a valid phone number in E.164 format (e.g., +1234567890).";
                }
            }
        });
    }
});








