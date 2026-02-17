import { MessageSquare } from 'lucide-react';

export function EmptyState() {
  return (
    <div className="flex-1 flex flex-col items-center justify-center gap-4 text-center px-6">
      <div className="size-16 rounded-full bg-white/5 border border-white/10 flex items-center justify-center">
        <MessageSquare className="size-8 text-primary/60" />
      </div>
      <div>
        <h3 className="text-lg font-semibold text-gray-300 mb-1">No conversation yet</h3>
        <p className="text-sm text-gray-500 max-w-sm">
          Ask about any player, team, or match to get started with live football statistics.
        </p>
      </div>
    </div>
  );
}
