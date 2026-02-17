import { useState, useEffect, useRef, type FormEvent } from 'react';
import { Background } from '@/components/layout/Background';
import { Header } from '@/components/layout/Header';
import { MessageBubble } from '@/components/chat/MessageBubble';
import { Button } from '@/components/ui/button';
import type { Message } from '@/types';
import { Send, Mic, Lock } from 'lucide-react';
import { getCurrentTime } from '@/lib/utils';

const INITIAL_MESSAGES: Message[] = [
  {
    id: '1',
    role: 'user',
    type: 'text',
    content: 'Dame las estadisticas de Haaland esta temporada.',
    timestamp: '21:45',
  },
  {
    id: '2',
    role: 'assistant',
    type: 'stats-card',
    timestamp: '21:45',
    data: {
      title: 'GOLES',
      value: 32,
      leagues: [
        { name: 'Premier League', value: 25 },
        { name: 'Champions', value: 7 },
      ],
      insight:
        'Haaland esta superando su xG (Goles Esperados) esta temporada con una eficiencia del',
      efficiency: 98,
    },
  },
];

export function ChatPage() {
  const [messages, setMessages] = useState<Message[]>(INITIAL_MESSAGES);
  const [inputValue, setInputValue] = useState('');
  const messagesEndRef = useRef<HTMLDivElement>(null);

  const scrollToBottom = () => {
    messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  };

  useEffect(() => {
    scrollToBottom();
  }, [messages]);

  const handleSend = async (e: FormEvent) => {
    e.preventDefault();
    if (!inputValue.trim()) return;

    const userTimestamp = getCurrentTime();

    const newMessage: Message = {
      id: Date.now().toString(),
      role: 'user',
      type: 'text',
      content: inputValue,
      timestamp: userTimestamp,
    };

    setMessages((prev) => [...prev, newMessage]);
    const currentInput = inputValue;
    setInputValue('');

    try {
      const apiUrl = import.meta.env.VITE_API_URL || '';
      const response = await fetch(`${apiUrl}/api/chat`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ content: currentInput }),
      });

      const text = await response.text();

      if (!response.ok) throw new Error(`Error API: ${response.status} ${text}`);

      const data = JSON.parse(text);

      const botMessage: Message = {
        id: (Date.now() + 1).toString(),
        role: 'assistant',
        type: 'text',
        content: data.response,
        timestamp: getCurrentTime(),
      };

      setMessages((prev) => [...prev, botMessage]);
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div className="relative h-screen w-full flex flex-col items-center justify-center p-4 md:p-8 font-sans">
      <Background />

      <Header />

      <main className="w-full max-w-[960px] flex-1 flex flex-col bg-black/75 backdrop-blur-xl rounded-2xl border border-primary/50 shadow-[0_0_25px_rgba(77,142,255,0.15)] overflow-hidden relative group z-10 transition-all duration-500">
        <div className="absolute inset-0 rounded-2xl border border-primary/30 pointer-events-none shadow-[inset_0_0_20px_rgba(77,142,255,0.1)]"></div>

        <div className="flex-1 overflow-y-auto p-6 md:p-10 space-y-8 scrollbar-hide relative z-10">
          <div className="flex justify-center mb-8 sticky top-0 z-20">
            <div className="px-4 py-1.5 rounded-full bg-white/5 border border-white/10 text-xs font-medium text-gray-400 uppercase tracking-widest backdrop-blur-md shadow-lg">
              Live Match Intelligence
            </div>
          </div>

          {messages.map((msg) => (
            <MessageBubble key={msg.id} message={msg} />
          ))}
          <div ref={messagesEndRef} />
        </div>

        <div className="p-5 md:p-6 bg-white/5 backdrop-blur-xl border-t border-white/10 relative z-20">
          <form onSubmit={handleSend} className="flex gap-4 items-center relative">
            <div className="relative flex-1 group/input">
              <div className="absolute inset-0 bg-gradient-to-r from-primary/20 to-transparent opacity-0 group-hover/input:opacity-100 transition-opacity rounded-xl pointer-events-none"></div>
              <input
                type="text"
                value={inputValue}
                onChange={(e) => setInputValue(e.target.value)}
                className="w-full bg-black/40 border border-white/10 text-white placeholder-gray-500 text-sm rounded-xl px-4 py-3.5 pl-5 focus:outline-none focus:border-primary focus:ring-1 focus:ring-primary/50 transition-all font-display backdrop-blur-sm"
                placeholder="Pregunta sobre un jugador, equipo o partido..."
              />
              <div className="absolute right-3 top-1/2 -translate-y-1/2 flex gap-2">
                <button type="button" className="text-gray-500 hover:text-white transition-colors p-1">
                  <Mic className="w-5 h-5" />
                </button>
              </div>
            </div>

            <Button type="submit" size="icon" className="size-[46px] rounded-xl shrink-0">
              <Send className="w-5 h-5" />
            </Button>
          </form>

          <div className="mt-3 flex justify-center gap-6">
            <span className="text-[10px] text-gray-500 font-mono tracking-widest uppercase flex items-center gap-1.5">
              <span className="relative flex h-2 w-2">
                <span className="animate-ping absolute inline-flex h-full w-full rounded-full bg-green-400 opacity-75"></span>
                <span className="relative inline-flex rounded-full h-2 w-2 bg-green-500"></span>
              </span>
              Live Data
            </span>
            <span className="text-[10px] text-gray-500 font-mono tracking-widest uppercase flex items-center gap-1.5">
              <Lock className="w-3 h-3" /> Encrypted
            </span>
          </div>
        </div>
      </main>

      <div className="mt-4 text-center z-10">
        <p className="text-white/20 text-[10px] tracking-[0.3em] font-mono">OFFICIAL MATCH DATA PARTNER</p>
      </div>
    </div>
  );
}
