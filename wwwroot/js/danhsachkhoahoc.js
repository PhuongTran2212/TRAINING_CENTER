// Thêm animation và hiệu ứng cho trang đăng ký khóa học
document.addEventListener("DOMContentLoaded", () => {
    // Thêm animation order cho các card khóa học
    const courseCards = document.querySelectorAll(".col-lg-4.col-md-6.mb-4")
    courseCards.forEach((card, index) => {
      card.style.setProperty("--animation-order", index)
    })
  
    // Thêm hiệu ứng hover cho các card
    const allCards = document.querySelectorAll(".card")
    allCards.forEach((card) => {
      card.addEventListener("mouseenter", function () {
        this.style.transform = "translateY(-10px)"
        this.style.boxShadow = "0 15px 30px rgba(83, 72, 89, 0.1)"
      })
  
      card.addEventListener("mouseleave", function () {
        this.style.transform = "translateY(0)"
        this.style.boxShadow = "0 5px 15px rgba(0, 0, 0, 0.05)"
      })
    })
  
    // Thêm hiệu ứng ripple cho các nút
    const buttons = document.querySelectorAll(".btn")
    buttons.forEach((button) => {
      button.addEventListener("click", function (e) {
        // Bỏ qua nếu là nút disabled
        if (this.classList.contains("disabled")) return
  
        const x = e.clientX - e.target.getBoundingClientRect().left
        const y = e.clientY - e.target.getBoundingClientRect().top
  
        const ripple = document.createElement("span")
        ripple.classList.add("ripple")
        ripple.style.left = `${x}px`
        ripple.style.top = `${y}px`
  
        this.appendChild(ripple)
  
        setTimeout(() => {
          ripple.remove()
        }, 600)
      })
    })
  })
  