import { useChatStore } from '@/store/chat-store';
import { Button } from '@/components/ui/button';
import { cn } from '@/lib/utils';
import { MessageSquarePlus, X } from 'lucide-react';

interface ConversationSidebarProps {
  open: boolean;
  onClose: () => void;
}

function SkeletonItem() {
  return (
    <div className="px-3 py-3 animate-pulse">
      <div className="h-4 bg-white/10 rounded w-3/4 mb-2" />
      <div className="h-3 bg-white/10 rounded w-1/2" />
    </div>
  );
}

function formatRelativeTime(dateStr: string | null): string {
  if (!dateStr) return '';
  const date = new Date(dateStr);
  const now = new Date();
  const diffMs = now.getTime() - date.getTime();
  const diffMins = Math.floor(diffMs / 60000);

  if (diffMins < 1) return 'just now';
  if (diffMins < 60) return `${diffMins}m ago`;

  const diffHours = Math.floor(diffMins / 60);
  if (diffHours < 24) return `${diffHours}h ago`;

  const diffDays = Math.floor(diffHours / 24);
  if (diffDays < 7) return `${diffDays}d ago`;

  return date.toLocaleDateString('en-US', { month: 'short', day: 'numeric' });
}

export function ConversationSidebar({ open, onClose }: ConversationSidebarProps) {
  const {
    conversations,
    activeConversationId,
    isLoadingConversations,
    selectConversation,
    startNewConversation,
  } = useChatStore();

  const handleNewConversation = () => {
    startNewConversation();
    onClose();
  };

  const handleSelectConversation = (id: string) => {
    selectConversation(id);
    onClose();
  };

  return (
    <>
      {open && (
        <div className="fixed inset-0 bg-black/50 z-30 md:hidden" onClick={onClose} />
      )}

      <aside
        className={cn(
          'flex flex-col bg-black/80 backdrop-blur-xl border-r border-white/10 h-full z-40 transition-transform duration-300',
          'fixed md:relative w-72 md:w-64 shrink-0',
          open ? 'translate-x-0' : '-translate-x-full md:translate-x-0',
        )}
      >
        <div className="flex items-center justify-between p-4 border-b border-white/10">
          <span className="text-sm font-semibold text-gray-300 uppercase tracking-wider">
            Conversations
          </span>
          <div className="flex items-center gap-1">
            <Button
              variant="ghost"
              size="icon"
              className="size-8 text-gray-400 hover:text-white"
              onClick={handleNewConversation}
            >
              <MessageSquarePlus className="size-4" />
            </Button>
            <Button
              variant="ghost"
              size="icon"
              className="size-8 text-gray-400 hover:text-white md:hidden"
              onClick={onClose}
            >
              <X className="size-4" />
            </Button>
          </div>
        </div>

        <div className="flex-1 overflow-y-auto">
          {isLoadingConversations ? (
            <>
              <SkeletonItem />
              <SkeletonItem />
              <SkeletonItem />
            </>
          ) : conversations.length === 0 ? (
            <div className="p-4 text-center text-gray-500 text-sm">
              No conversations yet
            </div>
          ) : (
            conversations.map((conv) => (
              <button
                key={conv.id}
                onClick={() => handleSelectConversation(conv.id)}
                className={cn(
                  'w-full text-left px-3 py-3 border-b border-white/5 hover:bg-white/5 transition-colors',
                  activeConversationId === conv.id && 'bg-white/10 border-l-2 border-l-primary',
                )}
              >
                <p className="text-sm text-gray-200 truncate">{conv.title}</p>
                <p className="text-xs text-gray-500 mt-1">
                  {formatRelativeTime(conv.updatedAt ?? conv.createdAt)}
                </p>
              </button>
            ))
          )}
        </div>
      </aside>
    </>
  );
}
