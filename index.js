const baseUrl = "http://localhost:5157/api/User";

document.getElementById("submit").addEventListener("click", async (e) => {
  e.preventDefault();
  const email = document.getElementById("email").value;
  const password = document.getElementById("password").value;

  const formData = { email, password };

  const res = await fetch(`${baseUrl}/login`, {
    method: "POST",
    headers: {
      "content-type": "application/json",
    },
    body: JSON.stringify(formData),
  });

  const data = await res.json();

  localStorage.setItem("token" , data.payload)
  localStorage.setItem("userId" , data.userId)
  window.alert(data.message)


});
