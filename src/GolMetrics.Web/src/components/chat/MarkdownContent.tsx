import ReactMarkdown from 'react-markdown';
import remarkGfm from 'remark-gfm';

interface MarkdownContentProps {
  content: string;
}

export function MarkdownContent({ content }: MarkdownContentProps) {
  return (
    <ReactMarkdown
      remarkPlugins={[remarkGfm]}
      components={{
        table: ({ children }) => (
          <div className="overflow-x-auto my-2">
            <table className="min-w-full text-sm border-collapse border border-white/10">
              {children}
            </table>
          </div>
        ),
        thead: ({ children }) => (
          <thead className="bg-white/5">{children}</thead>
        ),
        th: ({ children }) => (
          <th className="px-3 py-2 text-left text-xs font-semibold text-gray-300 border border-white/10">
            {children}
          </th>
        ),
        td: ({ children }) => (
          <td className="px-3 py-2 text-gray-200 border border-white/10">{children}</td>
        ),
        p: ({ children }) => <p className="mb-2 last:mb-0">{children}</p>,
        ul: ({ children }) => <ul className="list-disc pl-5 mb-2 space-y-1">{children}</ul>,
        ol: ({ children }) => <ol className="list-decimal pl-5 mb-2 space-y-1">{children}</ol>,
        strong: ({ children }) => <strong className="font-bold text-white">{children}</strong>,
        code: ({ children, className }) => {
          const isBlock = className?.includes('language-');
          if (isBlock) {
            return (
              <code className="block bg-black/40 rounded-lg p-3 my-2 text-sm overflow-x-auto font-mono text-gray-200">
                {children}
              </code>
            );
          }
          return (
            <code className="bg-white/10 rounded px-1.5 py-0.5 text-sm font-mono">
              {children}
            </code>
          );
        },
        pre: ({ children }) => <pre className="my-2">{children}</pre>,
        a: ({ href, children }) => (
          <a
            href={href}
            target="_blank"
            rel="noopener noreferrer"
            className="text-primary underline hover:text-primary/80"
          >
            {children}
          </a>
        ),
      }}
    >
      {content}
    </ReactMarkdown>
  );
}
