''' 
Episode 10
https://www.youtube.com/watch?v=AdG_ITCFHDI
Explosions

# Background music: Frozen Jam by tgfcoder <https://twitter.com/tgfcoder> licensed under CC-BY-3 
'''
# import libraries
import pygame, os, math, random
import shared, player, mob, bullet, shield, explosion

class data():
	new_bullet_timer:float  = 0
	allow_new_bullet:bool   = True
	background:object		= None
	background_rect:object  = None
	player_img:object       = None
	meteor_img:object       = None
	bullet_img:object       = None
	shield_bar:object	    = None
	death_explosion:object  = None
	die_snd_channel:object  = None

mobs:list                 = []
bullets:list              = []
meteor_images             = []
new_bullet_interval:float = 0.2
sounds:dict               = {}
explosion_anim:dict       = {}
explosions:list           = []

def circle_collides(circle1:object, circle2:object) -> bool:
	# pygame.draw.circle(screen, (r,g,b), (x, y), R, w) #(r, g, b) is color, (x, y) is center, R is radius and w is the thickness of the circle border.
	# get distance between the circle's centers
	# use the Pythagorean Theorem to compute the distance
	distX = circle1.x - circle2.x
	distY = circle1.y - circle2.y
	distance = math.sqrt((distX * distX) + (distY * distY))	
	# if the distance is less than the sum of the circle's
	# radii, the circles are touching!
	r1 = circle1.radius
	r2 = circle2.radius
	if distance <= circle1.radius + circle2.radius:
		return True

	return False 

def collides(rect1:object, rect2:object) -> bool:
	''' check whether rectangles are NOT colliding '''
	if rect1.x > rect2.x + rect2.width or rect2.x > rect1.x + rect1.width:
		return False
	if rect1.y > rect2.y + rect2.height or rect2.y > rect1.y + rect1.height:
		return False
	return True

def process_events() -> (object, object, bool):
	''' get keyboard input and check if user closed window '''
	key_down = None
	quit = False
	for event in pygame.event.get():				# process events
		if event.type == pygame.QUIT:				# close button clicked
			quit = True
		elif event.type == pygame.KEYDOWN:			# single press of any key
			key_down = event.key	

	keystate = pygame.key.get_pressed()				# get keypressed events
	if keystate[pygame.K_ESCAPE]:					# player pressing escape continuously
		quit = True

	return keystate, key_down, quit					# usage: if key_down == pygame.K_RETURN:, if keystate[pygame.K_UP]:

def check_mob_bullet_collisions(dt) -> None:
	''' check if any bullets are colliding with any Mobs '''
	for i in range(len(bullets) -1, -1, -1):
		destroy = False
		for j in range(len(mobs) -1, -1, -1):
			#destroy set to true if rectangles are colliding (bullet + Mob)
			try:
				#destroy = collides(bullets[i].get_rect() , mobs[j].get_rect())
				destroy = circle_collides(bullets[i].get_circle(), mobs[j].get_circle())
			except:
				pass					
			if destroy:
				radius = mobs[j].get_circle().radius
				shared.score += 50 - radius				# higher score for small meteor
				if shared.audio_present:
					sounds["shoot"].stop()
					sounds[random.choice(["expl1","expl2"])].play()				
				bullets.pop()
				explosion_size = 'sm'
				if radius > 25:
					explosion_size = 'lg'
				explosions.append(explosion.Explosion(explosion_anim, mobs[j].get_rect().center, explosion_size))
				mobs[j].reset()
				if len(bullets) == 0:
					break	
				
	''' update bullets '''
	for i in range(len(bullets) - 1, -1, -1):
		if not bullets[i].update(dt):
			bullets.pop()

def check_mob_player_collisions(keystate:object, dt:float) -> None:
	''' check if mob hit player '''
	if player.alive:
		player.update(keystate, dt)
	else:
		if data.death_explosion == None:
			if not data.die_snd_channel.get_busy():
				shared.gamestate = shared.gamestates["quit"]
	for mob in mobs:
		mob.update(dt)
		''' check if any mobs colliding with Player '''
		#if collides(mob.get_rect(), player.rect):
		if circle_collides(mob.get_circle(), player.circle):
			mob.reset()
			if shared.audio_present:
				sounds['shoot'].stop()
				sounds["expl3"].play()			
			player.shield -= mob.get_circle().radius	# reduce player shield
			if player.shield <= 0:			# shield gone
				player.alive = False
				if shared.audio_present:
					data.die_snd_channel.play(sounds["die"])
				data.death_explosion = explosion.Explosion(explosion_anim, player.rect.center, 'player')			

def shoot() -> None:
	''' Create a new bullet at player's position '''
	if data.allow_new_bullet:
		if shared.audio_present:
			sounds["shoot"].stop()		
		newBullet = bullet.Bullet(data.bullet_img, player.rect.centerx, player.rect.top)
		bullets.append(newBullet)
		data.new_bullet_timer = 0
		data.allow_new_bullet = False

		if shared.audio_present:
			sounds["shoot"].play()

def update_explosions(dt:float) -> None:
	''' update explosions '''
	for i in range(len(explosions) - 1, -1, -1):
		if not explosions[i].update(dt):
			explosions.pop()
	if data.death_explosion != None:
		if not data.death_explosion.update(dt):
			data.death_explosion = None

def load_animations() -> None:
	''' load explosion images and scale for second list '''
	img_dir = os.path.join(shared.game_folder, "img")
	explosion_anim['lg'] = [] # large explosion empty list
	explosion_anim['sm'] = [] # small explosion empty list
	explosion_anim['player'] = [] # small explosion empty list
	for i in range(9):
		filename = f'regularExplosion0{i}.png' # clever way of adding sequential filenames
		# could also use: filename = 'regularExplosion0' + str(i) + '.png'
		img = pygame.image.load(os.path.join(img_dir, filename)).convert()
		img.set_colorkey(shared.BLACK)
		img_lg = pygame.transform.scale(img, (75, 75)) # create large explosion list (modify sizes to suit)
		explosion_anim['lg'].append(img_lg)
		img_sm = pygame.transform.scale(img, (32, 32)) # create small explosion list
		explosion_anim['sm'].append(img_sm)
		filename = f'sonicExplosion0{i}.png'
		img = pygame.image.load(os.path.join(img_dir, filename)).convert()
		img.set_colorkey(shared.BLACK)
		explosion_anim['player'].append(img)			

def load_images() -> None:
	img_dir = os.path.join(shared.game_folder, "img")
	data.background = pygame.image.load(os.path.join(img_dir, "starfield.png")).convert()
	data.background_rect = data.background.get_rect()
	data.player_img = pygame.image.load(os.path.join(img_dir, "playerShip1_orange.png")).convert()
	data.meteor_img = pygame.image.load(os.path.join(img_dir, "meteorBrown_med1.png")).convert()
	data.bullet_img = pygame.image.load(os.path.join(img_dir, "laserRed16.png")).convert() 

	meteor_list = ['meteorBrown_big1.png', 'meteorBrown_big2.png', 'meteorBrown_med1.png', 'meteorBrown_med3.png',
				   'meteorBrown_small1.png', 'meteorBrown_small2.png', 'meteorBrown_tiny1.png']
	for img in meteor_list:
		meteor_images.append(pygame.image.load(os.path.join(img_dir, img)).convert())	

def load_audio() -> None:
	if shared.audio_present:
		snd_dir = os.path.join(shared.game_folder, "snd")
		sounds.update(
			{"shoot" : pygame.mixer.Sound(os.path.join(snd_dir,'Laser_Shoot6.wav')),
			 "shield": pygame.mixer.Sound(os.path.join(snd_dir,'pow4.wav')),
			 "power" : pygame.mixer.Sound(os.path.join(snd_dir,'pow5.wav')),
			 "die"   : pygame.mixer.Sound(os.path.join(snd_dir,'rumble1.ogg')),
			 "expl1" : pygame.mixer.Sound(os.path.join(snd_dir,'expl3.wav')),
			 "expl2" : pygame.mixer.Sound(os.path.join(snd_dir,'expl6.wav')),
			 "expl3" : pygame.mixer.Sound(os.path.join(snd_dir,'Explosion5.wav'))
			 })	
		'''
		get more control over the die_sound effect
		If more channels needed, eg 16: (8 is the default)
		pygame.mixer.set_num_channels(16)
		use any channel, 7 chosen here:
		'''
		data.die_snd_channel = pygame.mixer.Channel(7)
		# load background music
		pygame.mixer.music.load(os.path.join(snd_dir, 'FrozenJam.ogg'))
		# reduce volume
		for key in sounds:
			sounds[key].set_volume(0.2)
			
		pygame.mixer.music.set_volume(0.2)	
		pygame.mixer.music.play(loops = -1)			# start background music if loaded

def load() -> None:
	''' Setup pygame '''
	shared.game_folder = os.getcwd() 					# current directory
	os.environ["SDL_VIDEO_CENTERED"] = "1" 				# Centre the Pygame window on screen
	pygame.init()       								# initialise pygame and game window
	try:
		pygame.mixer.init() 							# start pygame sound library
	except:
		shared.audio_present = False					# audio driver not installed
	shared.screen = pygame.display.set_mode((shared.WIDTH, shared.HEIGHT)) 
	pygame.display.set_caption(shared.window_title) 	# The window title
	shared.clock = pygame.time.Clock() 					# Keep track of framerate etc

	''' Load all game assets '''
	shared.debug = False
	load_images()
	load_audio()
	load_animations()
	
	player.init(data.player_img, 500, 0.5)
	for i in range(8):									# make 8 mobs
		mobs.append(mob.Mob(meteor_images))

	shared.score = 0
	data.shield_bar = shield.Shield(5, 5, shared.WHITE, shared.GREEN)	
	
def update() -> None:
	'''
	Update all game items and keyboard input
	delta-time used in Love2D and Monogame is measured in seconds
	Can be obtained from clock.tick() in ms
	'''
	dt = shared.clock.tick(shared.FPS) / 1000				# update clock. dt can be passed to other update functions

	data.new_bullet_timer += dt
	if data.new_bullet_timer >= new_bullet_interval:
		data.allow_new_bullet = True
		data.new_bullet_timer = 0

	keystate, key_down, quit = process_events()				# get keypressed, keydown and close/esc user choice
	if quit:
		shared.gamestate = shared.gamestates["quit"]		# set gamestate to quit
	else:
		if shared.gamestate == shared.gamestates["play"]:
			if key_down == pygame.K_SPACE or keystate[pygame.K_SPACE]:
				shoot()
			check_mob_player_collisions(keystate, dt)		
			check_mob_bullet_collisions(dt)
			update_explosions(dt)
			data.shield_bar.update(player.shield)	
	    
def draw() -> None:
	''' Draw background and all active sprites '''
	shared.screen.fill(shared.BLACK)					# make screen black
	shared.screen.blit(data.background, data.background_rect)
	if shared.gamestate == shared.gamestates["play"]:
		if data.death_explosion == None:
			player.draw()
		else:
			data.death_explosion.draw()				
		for mob in mobs:
			mob.draw()
		for bullet in bullets:
			bullet.draw()
		for explosion in explosions:
			explosion.draw()
		
	#draw the score
	# draw_text(screen:object, text:str, size:int, x:int, y:int, align:str = 'centre', colour:tuple = WHITE)
	shared.draw_text(shared.screen, str(shared.score), 18, shared.WIDTH / 2, 10)
	if shared.debug:
		shared.draw_text(shared.screen, "Debug mode", 18, 10, shared.HEIGHT - 24, "left", shared.YELLOW)
	# draw shield bar
	data.shield_bar.draw()

	pygame.display.flip()								# flip display to make it visible
	
def main() -> None:
	''' Run game loop and call other functions from here '''
	shared.WIDTH = 480									# default screen width: alter as required
	shared.HEIGHT = 600									# default screen height: alter as required
	shared.window_title = "Shmup!"						# default window title: change as required	
	load()												# setup window and game assets

	''' gameloop '''
	shared.gamestate = shared.gamestates["play"]		# gamestate starting at 1 ('play': no menu)
	while shared.gamestate < shared.gamestates["quit"]:	# 3 = quit
		update()										# go through update functions
		draw()											# go through draw functions

	pygame.quit()

main()