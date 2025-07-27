let quizData = null;
let currentQuestionIndex = 0;
let score = 0;

document.addEventListener("DOMContentLoaded", () => {
    // φόρμα login
    if (document.getElementById("loginForm")) {
        setupLogin();
    }

    // σελίδα του quiz
    if (document.getElementById("quizTitle")) {
        setupQuiz();
    }
});

function setupLogin()
{
    const form = document.getElementById('loginForm');
    const messageDiv = document.getElementById('message');

    form.addEventListener('submit', async (e) => {
        e.preventDefault();

        const username = document.getElementById('username').value.trim();
        const password = parseInt(document.getElementById('password').value.trim());

        try {
            const response = await fetch('https://localhost:5001/api/login', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ username, password })
            });

            const data = await response.json();

            if (response.ok) {
                messageDiv.style.color = 'green';
                messageDiv.textContent = data.message;

                setTimeout(() => {
                    window.location.href = 'quiz.html';
                }, 1500);
            } else {
                messageDiv.style.color = 'red';

                if (data.errors && Array.isArray(data.errors)) {
                    messageDiv.innerHTML = data.errors.map(err => `<div>• ${err}</div>`).join('');
                } else {
                    messageDiv.textContent = data.message || "Login failed.";
                }
            }

        } catch (error) {
            messageDiv.style.color = 'red';
            messageDiv.textContent = "Error connecting to server.";
            console.error(error);
        }
    });
}
function setupQuiz() {
    loadQuiz();
    document.getElementById("submitBtn").addEventListener("click", handleSubmit);
}

async function loadQuiz() {
    try {
        const response = await fetch("https://localhost:5001/api/quiz");
        quizData = await response.json();

        document.getElementById("quizTitle").textContent = quizData.title;
        loadQuestion();
    } catch (error) {
        console.error("Failed to load quiz data", error);
    }
}

function loadQuestion() {
    const q = quizData.questions[currentQuestionIndex];
    document.getElementById("questionText").textContent = q.title;
    document.getElementById("quizImage").src = q.img !== "#####" ? q.img : "";

    const form = document.getElementById("answerForm");
    form.innerHTML = "";

    if (q.question_type === "multiplechoice-single") {
        q.possible_answers.forEach(ans => {
            const label = document.createElement("label");
            label.innerHTML = `
        <input type="radio" name="answer" value="${ans.a_id}"> ${ans.caption}
      `;
            form.appendChild(label);
            form.appendChild(document.createElement("br"));
        });
    }

    if (q.question_type === "multiplechoice-multiple") {
        q.possible_answers.forEach(ans => {
            const label = document.createElement("label");
            label.innerHTML = `
        <input type="checkbox" name="answer" value="${ans.a_id}"> ${ans.caption}
      `;
            form.appendChild(label);
            form.appendChild(document.createElement("br"));
        });
    }

    if (q.question_type === "truefalse") {
        ["true", "false"].forEach((val) => {
            const label = document.createElement("label");
            label.innerHTML = `
        <input type="radio" name="answer" value="${val}"> ${val[0].toUpperCase() + val.slice(1)}
      `;
            form.appendChild(label);
            form.appendChild(document.createElement("br"));
        });
    }
}

function handleSubmit() {
    const q = quizData.questions[currentQuestionIndex];
    const feedbackDiv = document.getElementById("feedback");
    const submitBtn = document.getElementById("submitBtn");

    submitBtn.disabled = true;

    let userAnswers = [];
    const inputs = document.querySelectorAll('input[name="answer"]:checked');
    inputs.forEach(input => userAnswers.push(input.value));

    let isCorrect = false;

    if (q.question_type === "multiplechoice-single" || q.question_type === "truefalse") {
        isCorrect = userAnswers.length === 1 && userAnswers[0] == q.correct_answer.toString();
    } else if (q.question_type === "multiplechoice-multiple") {
        const correct = q.correct_answer.map(x => x.toString()).sort().join(",");
        const given = userAnswers.sort().join(",");
        isCorrect = correct === given;
    }

    if (isCorrect) {
        score += q.points;
        feedbackDiv.style.color = "green";
        feedbackDiv.textContent = "Correct!";
    } else {
        feedbackDiv.style.color = "red";
        feedbackDiv.textContent = "Wrong!";
        highlightCorrectAnswer(q);
    }

    document.querySelectorAll('input[name="answer"]').forEach(input => input.disabled = true);

    setTimeout(() => {
        feedbackDiv.textContent = "";
        currentQuestionIndex++;
        if (currentQuestionIndex < quizData.questions.length) {
            loadQuestion();
            submitBtn.disabled = false; 
        } else {
            showFinalResult();
        }
    }, 3000);
}

function highlightCorrectAnswer(q) {
    const answerInputs = document.querySelectorAll('input[name="answer"]');

    // Μπορεί να είναι array ή string
    const correctAnswers = Array.isArray(q.correct_answer)
        ? q.correct_answer.map(x => x.toString())
        : [q.correct_answer.toString()];

    answerInputs.forEach(input => {
        const parentLabel = input.parentElement;
        const value = input.value;

        // Αν είναι η σωστή απάντηση
        if (correctAnswers.includes(value)) {
            parentLabel.classList.add("correct-answer");
        }

        // Αν είναι λάθος και είχε επιλεγεί
        else if (input.checked) {
            parentLabel.classList.add("wrong-answer");
        }
    });
}


async function showFinalResult() {
    try {
        // 1. Φόρτωσε τα αποτελέσματα από τον server
        const response = await fetch("https://localhost:5001/api/quiz/result");
        const resultData = await response.json();

        // 2. Υπολόγισε το ποσοστό σκορ
        const totalPoints = quizData.questions.reduce((sum, q) => sum + q.points, 0);
        const percentage = Math.round((score / totalPoints) * 100);

        // 3. Βρες το σωστό αποτέλεσμα από το result.json
        const result = resultData.results.find(r =>
            percentage >= r.minpoints && percentage <= r.maxpoints
        );

        // 4. Εμφάνισε το αποτέλεσμα στη σελίδα
        document.querySelector(".quiz-container").innerHTML = `
      <h2>Final Score: ${percentage}%</h2>
      <h3>${result.title}</h3>
      <p>${result.message}</p>
      ${result.img && result.img !== "#####"
                ? `<img src="${result.img}" alt="Result" style="max-width: 100%; margin-top: 1rem;" />`
                : ""}
      <br><br>
      <button onclick="location.href='login.html'">Return to Login</button>
    `;
    } catch (error) {
        console.error("Failed to load result data", error);
        document.querySelector(".quiz-container").innerHTML =
            "<p>Failed to load result. Please try again.</p>";
    }
}

