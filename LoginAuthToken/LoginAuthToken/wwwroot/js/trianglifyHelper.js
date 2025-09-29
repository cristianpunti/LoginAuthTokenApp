window.trianglifyHelper = {
    canvas: null,
    ctx: null,
    triangles: [],
    triangleColors: [],
    animationFrameId: null,
    resizeHandler: null,
    baseColors: [
        // 🔹 Azules (cuerpo principal) → con más peso (repetidos)
        [25, 55, 109],   // Azul oscuro
        [25, 55, 109],   // Repetido para más peso
        [33, 97, 171],   // Azul intenso
        [33, 97, 171],   // Repetido
        [72, 133, 237],  // Azul medio
        [72, 133, 237],  // Repetido
        [135, 206, 250], // Azul celeste

        // 🟢 Verdes (contraste)
        [34, 139, 34],
        [72, 160, 120],
        [144, 238, 144],

        // 🟡 Amarillos y dorados
        [251, 176, 59],
        [255, 215, 0],
        [255, 239, 120],

        // 🔴 Rojos y naranjas
        [220, 74, 57],
        [255, 87, 34],
        [255, 140, 0],

        // ⚪ Blancos y neutros
        [240, 240, 240],
        [200, 200, 200]
    ],

    /**
     * Generate canvas and initialize triangles
     */
    generateCanvas: function () {
        try {
            // Remove existing canvas if any
            if (this.canvas) {
                this.removeCanvas();
            }

            // Create canvas element
            this.canvas = document.createElement("canvas");
            this.canvas.id = "trianglify-overlay";
            Object.assign(this.canvas.style, {
                position: "fixed",
                top: "0",
                left: "0",
                width: "100vw",
                height: "100vh",
                zIndex: "-1",
                pointerEvents: "none"
            });
            document.body.appendChild(this.canvas);

            // Set canvas resolution
            this.canvas.width = window.innerWidth;
            this.canvas.height = window.innerHeight;
            this.ctx = this.canvas.getContext("2d");

            // Generate triangles and start animation
            this.generateTriangles();
            this.animate();

            // Attach resize handler (only adjusts canvas size, does NOT regenerate triangles)
            this.resizeHandler = () => this.resizeCanvas();
            window.addEventListener("resize", this.resizeHandler);

        } catch (err) {
            console.error("Error generating canvas:", err);
        }
    },

    /**
     * Generate triangle positions and assign base colors
     */
    generateTriangles: function () {
        const cols = Math.ceil(this.canvas.width / 90) + 1;
        const rows = Math.ceil(this.canvas.height / 90) + 1;

        this.triangles = [];
        this.triangleColors = [];

        const points = [];
        for (let y = 0; y <= rows; y++) {
            for (let x = 0; x <= cols; x++) {
                points.push({
                    x: x * 90 + Math.random() * 30,
                    y: y * 90 + Math.random() * 30
                });
            }
        }

        for (let y = 0; y < rows; y++) {
            for (let x = 0; x < cols; x++) {
                const i = y * (cols + 1) + x;
                const p0 = points[i];
                const p1 = points[i + 1];
                const p2 = points[i + cols + 1];
                const p3 = points[i + cols + 2];

                if (p1 && p2) {
                    this.triangles.push([p0, p1, p2]);
                    this.triangleColors.push(this.pickBaseColor());
                }
                if (p2 && p3) {
                    this.triangles.push([p1, p3, p2]);
                    this.triangleColors.push(this.pickBaseColor());
                }
            }
        }
    },

    /**
     * Pick a random base color from palette
     */
    pickBaseColor: function () {
        return this.baseColors[Math.floor(Math.random() * this.baseColors.length)];
    },

    /**
     * Animate triangles with sinusoidal offsets + smooth color shifts
     */
    animate: function () {
        const t = Date.now() / 1000;
        const ctx = this.ctx;
        ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);

        this.triangles.forEach((triangle, i) => {
            const pts = triangle.map((p, j) => {
                const offsetX = Math.sin(t * 2 + i * 0.3 + j) * 5;
                const offsetY = Math.cos(t * 2 + i * 0.4 + j) * 5;
                return { x: p.x + offsetX, y: p.y + offsetY };
            });

            const base = this.triangleColors[i];
            const brightness = (Math.sin(t + i) + 1) / 2 * 0.3 + 0.85;

            const r = Math.min(255, Math.max(0, Math.round(base[0] * brightness)));
            const g = Math.min(255, Math.max(0, Math.round(base[1] * brightness)));
            const b = Math.min(255, Math.max(0, Math.round(base[2] * brightness)));

            ctx.beginPath();
            ctx.moveTo(pts[0].x, pts[0].y);
            ctx.lineTo(pts[1].x, pts[1].y);
            ctx.lineTo(pts[2].x, pts[2].y);
            ctx.closePath();
            ctx.fillStyle = `rgb(${r},${g},${b})`;
            ctx.fill();
        });

        this.animationFrameId = requestAnimationFrame(this.animate.bind(this));
    },

    /**
     * Resize canvas to fill window without regenerating triangles
     */
    resizeCanvas: function () {
        if (!this.canvas) return;
        this.canvas.width = window.innerWidth;
        this.canvas.height = window.innerHeight;
    },

    /**
     * Remove canvas, cancel animation and cleanup listeners
     */
    removeCanvas: function () {
        if (this.canvas) this.canvas.remove();
        if (this.animationFrameId) cancelAnimationFrame(this.animationFrameId);

        if (this.resizeHandler) {
            window.removeEventListener("resize", this.resizeHandler);
            this.resizeHandler = null;
        }

        this.canvas = null;
        this.ctx = null;
        this.triangles = [];
        this.triangleColors = [];
        this.animationFrameId = null;
    }
};