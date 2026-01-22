import {Settings, User as UserIcon} from "lucide-react";
import {Button} from "@/components/ui/button";

export const Header = () => {
    return (
        <header className="mb-6 relative w-full max-w-[960px] flex items-center justify-between shrink-0 z-20">
            <div
                className="absolute top-1/2 left-0 -translate-y-1/2 w-[200px] h-[60px] bg-white/5 blur-2xl rounded-full pointer-events-none"></div>

            <div className="flex items-center gap-3 relative z-10">
                <div
                    className="size-8 rounded-full bg-white/10 flex items-center justify-center border border-white/20 shadow-[0_0_15px_rgba(255,255,255,0.2)]">
                    <svg
                        className="w-5 h-5 text-white"
                        fill="currentColor"
                        viewBox="0 0 24 24"
                    >
                        <path
                            d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-1 17.93c-3.95-.49-7-3.85-7-7.93 0-.62.08-1.21.21-1.79L9 15v1c0 1.1.9 2 2 2v1.93zm6.9-2.54c-.26-.81-1-1.39-1.9-1.39h-1v-3c0-.55-.45-1-1-1H8v-2h2c.55 0 1-.45 1-1V7h2c1.1 0 2-.9 2-2v-.41c2.93 1.19 5 4.06 5 7.41 0 2.08-.8 3.97-2.1 5.39z"/>
                    </svg>
                </div>
                <h1 className="text-2xl font-bold tracking-tight text-white drop-shadow-[0_0_8px_rgba(255,255,255,0.5)] font-display">
                    GOL <span className="text-primary font-black">METRICS</span>
                </h1>
            </div>

            <div className="flex gap-3">
                <Button variant="glass" size="icon" className="rounded-xl">
                    <Settings className="w-5 h-5"/>
                </Button>
                <Button variant="glass" size="icon" className="rounded-xl">
                    <UserIcon className="w-5 h-5"/>
                </Button>
            </div>
        </header>
    );
};
