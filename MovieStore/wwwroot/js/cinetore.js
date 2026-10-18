// cinetore.js - frontend interactions for the CINETORE / MovieStore app.
// Static movie data has been removed; content is now rendered server-side by Razor.

// ─── Utility ────────────────────────────────────────────────────────────────

function googleSearch(value) {
    return "https://www.google.com/search?q=" + encodeURIComponent(value + " actor");
}

// Actor images available locally in /imagesaces/
const availableCastImages = new Set([
    "adam-driver.jpg", "al-pacino.jpg", "ana-de-armas.jpg", "anna-paquin.jpg",
    "billy-crystal.jpg", "candice-bergen.jpg", "carrie-fisher.jpg", "chris-hemsworth.jpg",
    "daisy-edgar-jones.jpg", "dwayne-johnson.jpg", "enzo-vogrincic.jpg", "gal-gadot.jpg",
    "golshifteh-farahani.jpg", "jacob-batalon.jpg", "jamie-foxx.jpg", "jennifer-garner.jpg",
    "jennifer-lawrence.jpg", "joe-pesci.jpg", "john-boyega.jpg", "jonah-hill.jpg",
    "josh-lucas.jpg", "kiefer-sutherland.jpg", "laura-dern.jpg", "leonardo-dicaprio.jpg",
    "mari-natsuki.jpg", "mark-ruffalo.jpg", "meg-ryan.jpg", "meryl-streep.jpg",
    "patrick-dempsey.jpg", "patrick-wilson.jpg", "randeep-hooda.jpg", "ray-liotta.jpg",
    "reese-witherspoon.jpg", "rege-jean-page.jpg", "robert-de-niro.jpg", "rudhraksh-jaiswal.jpg",
    "ryan-gosling.jpg", "ryan-reynolds.jpg", "sadie-sink.jpg", "scarlett-johansson.jpg",
    "teyonah-parris.jpg", "tom-holland.jpg", "vera-farmiga.jpg", "walker-scobell.jpg",
    "zendaya.jpg"
]);

// ─── Notifications ──────────────────────────────────────────────────────────

function showNotification(message) {
    const notification = document.getElementById("cinetoreNotification");
    if (!notification) return;
    notification.textContent = message;
    notification.hidden = false;
    window.clearTimeout(showNotification.timeout);
    showNotification.timeout = window.setTimeout(function () {
        notification.hidden = true;
    }, 3500);
}

// ─── Floating CTA & Request Movie Modal ─────────────────────────────────────

function ensureFeedbackModals() {
    if (document.getElementById("requestMovieModal")) return;

    document.body.insertAdjacentHTML("beforeend", `<div class="cinetore-modal-backdrop" id="requestMovieModal" hidden>
        <section class="cinetore-modal" role="dialog" aria-modal="true" aria-labelledby="requestMovieHeading">
            <button type="button" class="modal-close" data-modal-close aria-label="Close request form"><span aria-hidden="true">&times;</span></button>
            <h2 id="requestMovieHeading">Request a Movie</h2>
            <form id="requestMovieForm" novalidate>
                <div class="modal-field">
                    <label for="requestMovieName">Movie Name</label>
                    <input type="text" id="requestMovieName" name="movieName" placeholder="Enter the movie name" required>
                </div>
                <div class="modal-field">
                    <label for="requestReleaseYear">Release Year</label>
                    <input type="text" id="requestReleaseYear" name="releaseYear" placeholder="Enter the release year" inputmode="numeric" maxlength="4" pattern="[0-9]{4}" required>
                </div>
                <div class="modal-field">
                    <label for="requestAdditionalInfo">Additional Information</label>
                    <textarea id="requestAdditionalInfo" name="additionalInfo" placeholder="Any additional details (optional)"></textarea>
                </div>
                <div class="modal-actions">
                    <button type="submit" class="btn btn-accent">Submit Request</button>
                </div>
                <p class="modal-message" id="requestMovieMessage" role="status" hidden></p>
            </form>
        </section>
    </div>`);

    document.body.insertAdjacentHTML("beforeend", `<div class="cinetore-notification" id="cinetoreNotification" role="status" aria-live="polite" hidden></div>`);
}

function setupFeedbackModals() {
    ensureFeedbackModals();

    const modalPairs = [
        {
            button: document.getElementById("requestMovieBtn"),
            modal: document.getElementById("requestMovieModal"),
            form: document.getElementById("requestMovieForm"),
            message: document.getElementById("requestMovieMessage")
        }
    ];

    let activeModal = null;
    let opener = null;

    function closeModal(modal, restoreFocus) {
        if (!modal) return;
        modal.hidden = true;
        document.body.classList.remove("modal-open");
        if (restoreFocus && opener) opener.focus();
        activeModal = null;
        opener = null;
    }

    function openModal(modal, button) {
        if (activeModal) closeModal(activeModal, false);
        activeModal = modal;
        opener = button;
        modal.hidden = false;
        document.body.classList.add("modal-open");
        const message = modal.querySelector(".modal-message");
        if (message) { message.hidden = true; message.textContent = ""; }
        modal.querySelector("input, select, textarea").focus();
    }

    modalPairs.forEach(function (pair) {
        if (!pair.button || !pair.modal || !pair.form) return;

        pair.button.addEventListener("click", function () {
            openModal(pair.modal, pair.button);
        });

        pair.modal.addEventListener("click", function (event) {
            if (event.target === pair.modal || event.target.closest("[data-modal-close]")) {
                closeModal(pair.modal, true);
            }
        });

        pair.form.addEventListener("submit", function (event) {
            event.preventDefault();

            if (pair.form.id === "requestMovieForm") {
                const name = document.getElementById("requestMovieName");
                const year = document.getElementById("requestReleaseYear");
                const yearText = year.value.trim();
                const yearValue = Number(yearText);
                const currentYear = new Date().getFullYear();

                name.setCustomValidity("");
                year.setCustomValidity("");

                if (!name.value.trim() || !/^\d{4}$/.test(yearText) || yearValue < 1888 || yearValue > currentYear + 5) {
                    name.setCustomValidity(name.value.trim() ? "" : "Enter a movie name.");
                    pair.form.reportValidity();
                    if (!/^\d{4}$/.test(yearText) || yearValue < 1888 || yearValue > currentYear + 5) {
                        year.setCustomValidity("Enter a valid four-digit release year.");
                        year.reportValidity();
                        year.setCustomValidity("");
                    }
                    return;
                }

                pair.form.reset();
                pair.message.hidden = true;
                closeModal(pair.modal, true);
                showNotification("Your movie request was submitted.");
                return;
            }

            pair.form.reset();
            pair.message.hidden = false;
        });
    });

    document.addEventListener("keydown", function (event) {
        if (event.key === "Escape" && activeModal) closeModal(activeModal, true);
    });
}

// ─── Floating CTA expand animation ──────────────────────────────────────────

function setupFloatingCTA() {
    const btn = document.getElementById("requestMovieBtn");
    if (!btn) return;
    btn.addEventListener("mouseenter", function () { btn.classList.add("expanded"); });
    btn.addEventListener("mouseleave", function () { btn.classList.remove("expanded"); });
}

// ─── Home page — live search filter ─────────────────────────────────────────

function setupHomePage() {
    const movieSearch = document.getElementById("movieSearch");
    const movieCards = Array.from(document.querySelectorAll(".movie-card"));
    const noMoviesMessage = document.getElementById("noMoviesMessage");

    if (!movieSearch || movieCards.length === 0) return;

    function filterMovies() {
        const query = movieSearch.value.trim().toLowerCase();
        let visibleMovies = 0;

        movieCards.forEach(function (movieCard) {
            const title = (movieCard.dataset.movieTitle || (movieCard.querySelector("img") ? movieCard.querySelector("img").alt : "")).toLowerCase();
            const column = movieCard.closest("[class*='col-']") || movieCard.parentElement;
            const isVisible = title.includes(query);

            if (column && column !== movieCard) {
                column.hidden = !isVisible;
            } else {
                movieCard.hidden = !isVisible;
            }

            if (isVisible) visibleMovies++;
        });

        if (noMoviesMessage) {
            noMoviesMessage.textContent = "No movies found.";
            noMoviesMessage.hidden = visibleMovies !== 0;
        }
    }

    movieSearch.addEventListener("input", filterMovies);
    filterMovies();
}

// ─── Genre page — client-side filter ────────────────────────────────────────

function setupGenrePage() {
    const filterButtons = Array.from(document.querySelectorAll(".genre-filter-button"));
    const movieColumns = Array.from(document.querySelectorAll(".genre-movie-column"));
    const emptyState = document.getElementById("genreEmptyState");

    if (filterButtons.length === 0 || movieColumns.length === 0) return;

    function selectGenre(selectedGenre) {
        filterButtons.forEach(function (button) {
            const isSelected = button.dataset.genre === selectedGenre;
            button.classList.toggle("active", isSelected);
            button.setAttribute("aria-pressed", String(isSelected));
        });

        let visible = 0;
        movieColumns.forEach(function (col) {
            const genre = col.dataset.genre || "";
            const show = selectedGenre === "All" || genre.toLowerCase().includes(selectedGenre.toLowerCase());
            col.hidden = !show;
            if (show) visible++;
        });

        const filterInput = document.getElementById("genreFilterInput");
        if (filterInput) {
            filterInput.value = (selectedGenre === "All") ? "" : selectedGenre;
        }

        if (emptyState) emptyState.hidden = visible !== 0;

        history.replaceState(null, "", "?genre=" + encodeURIComponent(selectedGenre));
    }

    filterButtons.forEach(function (button) {
        button.addEventListener("click", function () {
            selectGenre(button.dataset.genre);
        });
    });

    // Read genre from query string on page load
    const requestedGenre = new URLSearchParams(window.location.search).get("genre");
    const validGenres = filterButtons.map(function (b) { return b.dataset.genre; });
    const initialGenre = validGenres.includes(requestedGenre) ? requestedGenre : "All";
    selectGenre(initialGenre);
}

// ─── Theme toggle ────────────────────────────────────────────────────────────

function setupThemeToggle() {
    const themeToggle = document.getElementById("themeToggle");
    const themeIcon = document.getElementById("themeIcon");
    if (!themeToggle || !themeIcon) return;

    if (localStorage.getItem("cinetoreTheme") === "light") {
        document.body.classList.add("light-mode");
    }

    function updateThemeButton() {
        const isLightMode = document.body.classList.contains("light-mode");
        themeIcon.classList.toggle("fa-moon", isLightMode);
        themeIcon.classList.toggle("fa-sun", !isLightMode);
        themeToggle.setAttribute("aria-label", isLightMode ? "Switch to dark theme" : "Switch to light theme");
    }

    updateThemeButton();

    themeToggle.addEventListener("click", function () {
        document.body.classList.toggle("light-mode");
        localStorage.setItem("cinetoreTheme", document.body.classList.contains("light-mode") ? "light" : "dark");
        updateThemeButton();
    });
}

// ─── Social links placeholder ────────────────────────────────────────────────

function setupSocialLinks() {
    document.querySelectorAll("[data-social-placeholder]").forEach(function (link) {
        link.addEventListener("click", function (event) {
            event.preventDefault();
            alert(link.dataset.socialPlaceholder + " profile link is not configured yet.");
        });
    });
}

// ─── Watch Now / Download buttons ───────────────────────────────────────────

function setupMovieActions() {
    document.querySelectorAll(".watch-now").forEach(function (btn) {
        btn.addEventListener("click", function () {
            showNotification("Streaming is not configured yet. Coming soon!");
        });
    });
    document.querySelectorAll(".download-movie").forEach(function (btn) {
        btn.addEventListener("click", function () {
            showNotification("Download is not available yet. Coming soon!");
        });
    });
}

// ─── Bootstrap 3 nav toggle ──────────────────────────────────────────────────
// Bootstrap 3 collapses require data-toggle="collapse" handled by bootstrap.js (loaded from CDN).
// No extra code needed here.

// ─── Init ────────────────────────────────────────────────────────────────────

document.addEventListener("DOMContentLoaded", function () {
    setupThemeToggle();
    setupFeedbackModals();
    setupFloatingCTA();
    setupHomePage();
    setupGenrePage();
    setupMovieActions();
    setupSocialLinks();
});
