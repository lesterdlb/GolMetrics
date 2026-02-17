import type { Message } from '@/types';
import { MarkdownContent } from './MarkdownContent';
import { Bot, User } from 'lucide-react';
import { cn } from '@/lib/utils';

interface MessageBubbleProps {
  message: Message;
}

export function MessageBubble({ message }: MessageBubbleProps) {
  const isBot = message.role === 'assistant';

  return (
    <div className={cn('flex w-full gap-4', isBot ? 'justify-start' : 'justify-end')}>
      {isBot && (
        <div className="size-10 rounded-full bg-black border border-primary/50 flex items-center justify-center shrink-0 shadow-[0_0_15px_rgba(77,142,255,0.3)] z-10 relative">
          <Bot className="text-primary w-5 h-5" />
          <div className="absolute -bottom-1 -right-1 w-3 h-3 bg-green-500 rounded-full border-2 border-black animate-pulse" />
        </div>
      )}

      <div
        className={cn(
          'flex flex-col gap-1 max-w-[90%] md:max-w-[70%]',
          isBot ? 'items-start' : 'items-end',
        )}
      >
        <div className="flex items-center gap-2 mb-1 px-1">
          <span
            className={cn(
              'text-xs font-mono uppercase tracking-wider',
              isBot ? 'text-primary' : 'text-muted-foreground',
            )}
          >
            {isBot ? 'Gol Bot' : 'You'} &bull; {message.timestamp}
          </span>
          {isBot && (
            <span className="w-1.5 h-1.5 rounded-full bg-green-500 animate-pulse" />
          )}
        </div>

        <div
          className={cn(
            'px-5 py-4 rounded-2xl text-base font-normal leading-relaxed relative overflow-hidden shadow-lg border',
            isBot
              ? 'bg-secondary/80 border-white/5 text-gray-100 rounded-tl-sm'
              : 'bg-gradient-to-br from-[#2c3e50] to-[#34495e] border-white/10 text-white rounded-tr-sm',
          )}
        >
          {!isBot && (
            <div className="absolute top-0 right-0 w-full h-full bg-gradient-to-bl from-white/5 to-transparent pointer-events-none" />
          )}
          <div className="relative z-10">
            {isBot ? (
              <MarkdownContent content={message.content} />
            ) : (
              <p>{message.content}</p>
            )}
          </div>
        </div>
      </div>

      {!isBot && (
        <div className="size-10 rounded-full bg-gradient-to-b from-gray-700 to-gray-900 border border-gray-600 flex items-center justify-center shrink-0 shadow-lg">
          <User className="text-gray-400 w-5 h-5" />
        </div>
      )}
    </div>
  );
}
