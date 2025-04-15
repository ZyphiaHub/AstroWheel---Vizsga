<script lang="ts">
  import { goto } from "$app/navigation";
  import { sessionStore } from "$lib/stores/sessionStore";
  import { PUBLIC_SERVER_URL } from "$env/static/public";

  let email = "";
  let password = "";

  let isLoading = false;

  function validate() {
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailPattern || !emailPattern.test(email)) {
      return "E-mail is invalid!";
    }

    if (!password || password === "") {
      return "Password cannot be empty!";
    }
  }
  async function handleSignIn() {
    let errorMsg = validate();
    if (errorMsg) {
      alert(errorMsg);
      return;
    }
    try {
      isLoading = true;
      const res = await fetch(PUBLIC_SERVER_URL + "/api/Auth/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password }),
      });

      const data = await res.json();
      if (res.ok) {
        sessionStore.setToken(data.token);
        goto("/home");
      } else if (res.status === 401) {
        alert(data.error);
      } else {
        alert("Unexpected error!");
      }
    } catch (e) {
      alert("System is down. Please try again later.");
    }
    isLoading = false;
  }
</script>

<div class="login-container">
  <h2>Login</h2>
  <input
    class="input-field"
    type="text"
    placeholder="Email"
    bind:value={email}
  />
  <input
    class="input-field"
    type="password"
    placeholder="Password"
    bind:value={password}
  />

  {#if !isLoading}
    <button class="login-btn" on:click={handleSignIn}>Sign In</button>
  {:else}
    <p>Loading...</p>
  {/if}
</div>

<style>
  .login-container {
    background: rgba(0, 0, 0, 0.6);
    padding: 20px;
    border-radius: 20px;
    text-align: center;
    align-items: center;
    width: 320px;
  }

  .login-container h2 {
    margin-bottom: 15px;
    color: white;
  }

  .input-field {
    width: 94%;
    padding: 10px;
    margin: 5px 0;
    border: none;
    border-radius: 8px;
  }

  .login-btn {
    width: 100%;
    padding: 10px;
    background-color: #ff6600;
    color: white;
    border: none;
    border-radius: 8px;
    cursor: pointer;
    margin-top: 10px;
  }

  .login-btn:hover {
    background-color: #ff4500;
  }
</style>
