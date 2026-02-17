import { useState, useEffect, useRef, type FormEvent } from 'react';
import { Background } from '@/components/layout/Background';
import { Header } from '@/components/layout/Header';
import { MessageBubble } from '@/components/chat/MessageBubble';
import { ConversationSidebar } from '@/components/chat/ConversationSidebar';
import { TypingIndicator } from '@/components/chat/TypingIndicator';
import { EmptyState } from '@/components/chat/EmptyState';
import { Button } from '@/components/ui/button';
import { useChatStore } from '@/store/chat-store';
import { Send, Lock, Menu, Loader2 } from 'lucide-react';

export function ChatPage() {
  const {
    messages,
    activeConversationId,
    conversations,
    isSending,
    isLoadingMessages,
    fetchConversations,
    sendMessage,
  } = useChatStore();

  const [inputValue, setInputValue] = useState('');
  const [sidebarOpen, setSidebarOpen] = useState(false);
  const messagesEndRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    fetchConversations();
  }, [fetchConversations]);

  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  }, [messages]);

  const handleSend = async (e: FormEvent) => {
    e.preventDefault();
    if (!inputValue.trim() || isSending) return;

    const content = inputValue;
    setInputValue('');
    await sendMessage(content);
  };

  const showEmptyState = !activeConversationId && messages.length === 0 && conversations.length === 0;
  const showNewChatPrompt = !activeConversationId && messages.length === 0 && conversations.length > 0;

  return (
    <div className="relative h-screen w-full flex flex-col items-center justify-center p-4 md:p-8 font-sans">
      <Background />

      <Header />

      <div className="w-full max-w-[1200px] flex-1 flex overflow-hidden rounded-2xl border border-primary/50 shadow-[0_0_25px_rgba(77,142,255,0.15)] relative z-10">
        <ConversationSidebar open={sidebarOpen} onClose={() => setSidebarOpen(false)} />

        <main className="flex-1 flex flex-col bg-black/75 backdrop-blur-xl overflow-hidden relative group">
          <div className="absolute inset-0 rounded-r-2xl border border-primary/30 pointer-events-none shadow-[inset_0_0_20px_rgba(77,142,255,0.1)]" />

          <div className="flex items-center gap-2 px-4 py-2 border-b border-white/10 md:hidden relative z-20">
            <Button
              variant="ghost"
              size="icon"
              className="size-8 text-gray-400 hover:text-white"
              onClick={() => setSidebarOpen(true)}
            >
              <Menu className="size-5" />
            </Button>
            <span className="text-xs text-gray-500 uppercase tracking-widest">Chat</span>
          </div>

          <div className="flex-1 overflow-y-auto p-6 md:p-10 space-y-8 scrollbar-hide relative z-10">
            {isLoadingMessages ? (
              <div className="flex-1 flex items-center justify-center h-full">
                <Loader2 className="size-8 text-primary animate-spin" />
              </div>
            ) : showEmptyState || showNewChatPrompt ? (
              <EmptyState />
            ) : (
              <>
                <div className="flex justify-center mb-8 sticky top-0 z-20">
                  <div className="px-4 py-1.5 rounded-full bg-white/5 border border-white/10 text-xs font-medium text-gray-400 uppercase tracking-widest backdrop-blur-md shadow-lg">
                    Live Match Intelligence
                  </div>
                </div>

                {messages.map((msg) => (
                  <MessageBubble key={msg.id} message={msg} />
                ))}
                {isSending && <TypingIndicator />}
                <div ref={messagesEndRef} />
              </>
            )}
          </div>

          <div className="p-5 md:p-6 bg-white/5 backdrop-blur-xl border-t border-white/10 relative z-20">
            <form onSubmit={handleSend} className="flex gap-4 items-center relative">
              <div className="relative flex-1 group/input">
                <div className="absolute inset-0 bg-gradient-to-r from-primary/20 to-transparent opacity-0 group-hover/input:opacity-100 transition-opacity rounded-xl pointer-events-none" />
                <input
                  type="text"
                  value={inputValue}
                  onChange={(e) => setInputValue(e.target.value)}
                  disabled={isSending}
                  className="w-full bg-black/40 border border-white/10 text-white placeholder-gray-500 text-sm rounded-xl px-4 py-3.5 pl-5 focus:outline-none focus:border-primary focus:ring-1 focus:ring-primary/50 transition-all font-display backdrop-blur-sm disabled:opacity-50"
                  placeholder="Ask about a player, team, or match..."
                />
              </div>

              <Button
                type="submit"
                size="icon"
                className="size-[46px] rounded-xl shrink-0"
                disabled={isSending || !inputValue.trim()}
              >
                {isSending ? (
                  <Loader2 className="w-5 h-5 animate-spin" />
                ) : (
                  <Send className="w-5 h-5" />
                )}
              </Button>
            </form>

            <div className="mt-3 flex justify-center gap-6">
              <span className="text-[10px] text-gray-500 font-mono tracking-widest uppercase flex items-center gap-1.5">
                <span className="relative flex h-2 w-2">
                  <span className="animate-ping absolute inline-flex h-full w-full rounded-full bg-green-400 opacity-75" />
                  <span className="relative inline-flex rounded-full h-2 w-2 bg-green-500" />
                </span>
                Live Data
              </span>
              <span className="text-[10px] text-gray-500 font-mono tracking-widest uppercase flex items-center gap-1.5">
                <Lock className="w-3 h-3" /> Encrypted
              </span>
            </div>
          </div>
        </main>
      </div>

      <div className="mt-4 text-center z-10">
        <p className="text-white/20 text-[10px] tracking-[0.3em] font-mono">
          OFFICIAL MATCH DATA PARTNER
        </p>
      </div>
    </div>
  );
}
