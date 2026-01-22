export const Background = () => {
    return (
        <div className="absolute inset-0 z-0 pointer-events-none overflow-hidden bg-background">
            <div
                className="absolute inset-0 bg-[radial-gradient(circle_at_50%_0%,_#1a2f4a_0%,_#0a1628_60%,_#050c18_100%)]"></div>

            <div
                className="absolute inset-0 opacity-20 mix-blend-screen"
                style={{
                    backgroundImage: `
            radial-gradient(white, rgba(255,255,255,.2) 2px, transparent 3px),
            radial-gradient(white, rgba(255,255,255,.15) 1px, transparent 2px),
            radial-gradient(white, rgba(255,255,255,.1) 2px, transparent 3px)
          `,
                    backgroundSize: "550px 550px, 350px 350px, 250px 250px",
                    backgroundPosition: "0 0, 40px 60px, 130px 270px",
                }}
            ></div>

            <div
                className="absolute bottom-[-10%] left-1/2 -translate-x-1/2 w-[140%] h-[60%] bg-[radial-gradient(ellipse_at_center,_#152a45_0%,_transparent_70%)] opacity-40 blur-3xl"></div>
            <div
                className="absolute bottom-[-150px] left-[-100px] w-[800px] h-[500px] bg-primary/10 rounded-full blur-[100px] mix-blend-screen animate-pulse-slow"></div>
            <div
                className="absolute bottom-[-150px] right-[-100px] w-[800px] h-[500px] bg-primary/10 rounded-full blur-[100px] mix-blend-screen animate-pulse-slow"
                style={{animationDelay: "1s"}}
            ></div>
            <div
                className="absolute bottom-[-50px] left-1/2 -translate-x-1/2 w-[600px] h-[200px] bg-white/5 rounded-full blur-[80px]"></div>

            <div
                className="absolute bottom-0 w-full h-[30vh] flex justify-center items-end opacity-[0.07] pointer-events-none">
                <div
                    className="w-[90%] h-full border-x border-white/50 transform perspective-[500px] rotate-x-12 origin-bottom bg-[linear-gradient(0deg,transparent_24%,rgba(255,255,255,.3)_25%,rgba(255,255,255,.3)_26%,transparent_27%,transparent_74%,rgba(255,255,255,.3)_75%,rgba(255,255,255,.3)_76%,transparent_77%,transparent),linear-gradient(90deg,transparent_24%,rgba(255,255,255,.3)_25%,rgba(255,255,255,.3)_26%,transparent_27%,transparent_74%,rgba(255,255,255,.3)_75%,rgba(255,255,255,.3)_76%,transparent_77%,transparent)] bg-[length:50px_50px]"></div>
            </div>
        </div>
    );
};
