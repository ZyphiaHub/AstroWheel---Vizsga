<script>
  let {
    showModal = $bindable(),
    toggleImage = $bindable(),
    showFirstImage = $bindable(),
    isMultiPage = $bindable(),
    children,
  } = $props();

  let dialog = $state();

  $effect(() => {
    if (showModal) dialog.showModal();
  });
</script>

<!-- svelte-ignore a11y_click_events_have_key_events -->
<!-- svelte-ignore a11y_no_noninteractive_element_interactions -->
<dialog
  bind:this={dialog}
  onclose={() => (showModal = false)}
  onclick={(e) => {
    if (e.target === dialog) {
      dialog.close();
      showFirstImage = true;
    }
  }}
>
  <button
    class="close-btn"
    onclick={() => {
      dialog.close();
      showFirstImage = true;
    }}>✖</button
  >

  <div class="modal-content">
    <div class="image-container">
      {@render children?.()}
    </div>

    {#if isMultiPage}
      <div class="button-container">
        <button class="info-btn" onclick={() => toggleImage()}
          >{showFirstImage
            ? "Character Options >>"
            : "<< Character Info"}</button
        >
      </div>
    {/if}
  </div>
</dialog>

<style>
  dialog {
    max-width: 120em;
    border-radius: 0.2em;
    border: none;
    padding: 0;
  }
  dialog::backdrop {
    background: rgba(0, 0, 0, 0.3);
  }
  dialog > div {
    padding: 1em;
  }
  dialog[open] {
    animation: zoom 0.8s cubic-bezier(0.34, 1.56, 0.64, 1);
  }
  @keyframes zoom {
    from {
      transform: scale(0.1);
    }
    to {
      transform: scale(1);
    }
  }
  dialog[open]::backdrop {
    animation: fade 0.2s ease-out;
  }
  @keyframes fade {
    from {
      opacity: 0;
    }
    to {
      opacity: 1;
    }
  }
  .modal-content {
    display: flex;
    flex-direction: column;
    align-items: center;
  }
  .image-container {
    display: flex;
    justify-content: center;
  }
  .close-btn {
    position: absolute;
    top: 10px;
    right: 10px;
    background: #000000;
    color: white;
    border: none;
    padding: 8px 12px;
    font-size: 18px;
    border-radius: 50%;
    cursor: pointer;
    transition: background 0.3s;
  }

  .close-btn:hover {
    background: #eec77f;
  }

  .button-container {
    display: flex;
    justify-content: center;
    width: 100%;
    margin-top: 15px;
  }

  .info-btn {
    background: none;
    border: 2px solid #000000;
    color: #000000;
    padding: 10px 20px;
    font-size: 16px;
    cursor: pointer;
    transition: all 0.3s;
  }

  .info-btn:hover {
    background: #eec77f;
    color: white;
    border: 2px solid #eec77f;
  }
</style>
