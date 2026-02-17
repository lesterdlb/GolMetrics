import { Bot } from 'lucide-react';

export function TypingIndicator() {
  return (
    <div className="flex w-full gap-4 justify-start">
      <div className="size-10 rounded-full bg-black border border-primary/50 flex items-center justify-center shrink-0 shadow-[0_0_15px_rgba(77,142,255,0.3)] z-10 relative">
        <Bot className="text-primary w-5 h-5" />
        <div className="absolute -bottom-1 -right-1 w-3 h-3 bg-green-500 rounded-full border-2 border-black animate-pulse" />
      </div>

      <div className="flex flex-col gap-1 max-w-[90%] md:max-w-[70%] items-start">
        <div className="flex items-center gap-2 mb-1 px-1">
          <span className="text-xs font-mono uppercase tracking-wider text-primary">
            Gol Bot
          </span>
          <span className="w-1.5 h-1.5 rounded-full bg-green-500 animate-pulse" />
        </div>
        <div className="px-5 py-4 rounded-2xl rounded-tl-sm bg-secondary/80 border border-white/5 shadow-lg">
          <div className="flex gap-1.5 items-center">
            <span className="w-2 h-2 rounded-full bg-gray-400 animate-bounce [animation-delay:0ms]" />
            <span className="w-2 h-2 rounded-full bg-gray-400 animate-bounce [animation-delay:150ms]" />
            <span className="w-2 h-2 rounded-full bg-gray-400 animate-bounce [animation-delay:300ms]" />
          </div>
        </div>
      </div>
    </div>
  );
}
