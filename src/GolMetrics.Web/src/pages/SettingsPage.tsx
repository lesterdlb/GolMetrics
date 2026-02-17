import { useEffect, useState, type FormEvent } from 'react';
import { Background } from '@/components/layout/Background';
import { Header } from '@/components/layout/Header';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { useSettingsStore } from '@/store/settings-store';
import { Loader2, Eye, EyeOff, Key, Mail, Calendar, Shield } from 'lucide-react';

function ProfileSkeleton() {
  return (
    <div className="space-y-4 animate-pulse">
      <div className="h-4 w-48 bg-white/10 rounded" />
      <div className="h-4 w-32 bg-white/10 rounded" />
      <div className="h-4 w-24 bg-white/10 rounded" />
    </div>
  );
}

export function SettingsPage() {
  const { profile, isLoadingProfile, isSubmittingApiKey, fetchProfile, submitApiKey } =
    useSettingsStore();

  const [apiKey, setApiKey] = useState('');
  const [showApiKey, setShowApiKey] = useState(false);
  const [validationError, setValidationError] = useState('');

  useEffect(() => {
    fetchProfile();
  }, [fetchProfile]);

  async function handleSubmitApiKey(e: FormEvent) {
    e.preventDefault();
    setValidationError('');

    if (!apiKey.trim()) {
      setValidationError('API key is required.');
      return;
    }

    await submitApiKey(apiKey);
    setApiKey('');
  }

  const formattedDate = profile?.createdAt
    ? new Date(profile.createdAt).toLocaleDateString('en-US', {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
      })
    : '';

  return (
    <div className="relative h-screen w-full flex flex-col items-center p-4 md:p-8 font-sans">
      <Background />

      <Header />

      <div className="w-full max-w-[640px] flex-1 overflow-y-auto relative z-10 space-y-6">
        <div className="rounded-2xl border border-primary/50 bg-black/75 backdrop-blur-xl p-6 md:p-8 shadow-[0_0_25px_rgba(77,142,255,0.15)]">
          <h2 className="text-lg font-semibold text-white mb-6 flex items-center gap-2">
            <Shield className="size-5 text-primary" />
            Profile Information
          </h2>

          {isLoadingProfile ? (
            <ProfileSkeleton />
          ) : profile ? (
            <div className="space-y-4">
              <div className="flex items-center gap-3">
                <Mail className="size-4 text-gray-400 shrink-0" />
                <div>
                  <p className="text-xs text-gray-500 uppercase tracking-wider">Email</p>
                  <p className="text-white">{profile.email}</p>
                </div>
              </div>

              <div className="flex items-center gap-3">
                <Calendar className="size-4 text-gray-400 shrink-0" />
                <div>
                  <p className="text-xs text-gray-500 uppercase tracking-wider">Member since</p>
                  <p className="text-white">{formattedDate}</p>
                </div>
              </div>

              <div className="flex items-center gap-3">
                <Key className="size-4 text-gray-400 shrink-0" />
                <div>
                  <p className="text-xs text-gray-500 uppercase tracking-wider">API Key</p>
                  <span
                    className={`inline-block mt-1 px-2 py-0.5 rounded text-xs font-medium ${
                      profile.hasApiKey
                        ? 'bg-green-500/20 text-green-400 border border-green-500/30'
                        : 'bg-yellow-500/20 text-yellow-400 border border-yellow-500/30'
                    }`}
                  >
                    {profile.hasApiKey ? 'Configured' : 'Not configured'}
                  </span>
                </div>
              </div>
            </div>
          ) : null}
        </div>

        <div className="rounded-2xl border border-primary/50 bg-black/75 backdrop-blur-xl p-6 md:p-8 shadow-[0_0_25px_rgba(77,142,255,0.15)]">
          <h2 className="text-lg font-semibold text-white mb-2 flex items-center gap-2">
            <Key className="size-5 text-primary" />
            API Key Management
          </h2>
          <p className="text-sm text-gray-400 mb-6">
            Enter your API-Football key to use your own quota for football data requests.
          </p>

          <form onSubmit={handleSubmitApiKey} className="space-y-4">
            <div className="space-y-2">
              <Label htmlFor="apiKey" className="text-white/80">
                API-Football Key
              </Label>
              <div className="relative">
                <Input
                  id="apiKey"
                  type={showApiKey ? 'text' : 'password'}
                  placeholder="Enter your API key"
                  value={apiKey}
                  onChange={(e) => {
                    setApiKey(e.target.value);
                    if (validationError) setValidationError('');
                  }}
                  disabled={isSubmittingApiKey}
                  className="border-white/10 bg-white/5 text-white placeholder:text-white/30 pr-10"
                />
                <Button
                  type="button"
                  variant="ghost"
                  size="icon-xs"
                  className="absolute right-2 top-1/2 -translate-y-1/2 text-gray-400 hover:text-white"
                  onClick={() => setShowApiKey(!showApiKey)}
                  tabIndex={-1}
                >
                  {showApiKey ? <EyeOff className="size-4" /> : <Eye className="size-4" />}
                </Button>
              </div>
              {validationError && <p className="text-sm text-destructive">{validationError}</p>}
            </div>

            <Button type="submit" disabled={isSubmittingApiKey} className="w-full">
              {isSubmittingApiKey ? (
                <>
                  <Loader2 className="animate-spin" />
                  Validating...
                </>
              ) : (
                'Save API Key'
              )}
            </Button>
          </form>
        </div>
      </div>

      <div className="mt-4 text-center z-10">
        <p className="text-white/20 text-[10px] tracking-[0.3em] font-mono">
          OFFICIAL MATCH DATA PARTNER
        </p>
      </div>
    </div>
  );
}
