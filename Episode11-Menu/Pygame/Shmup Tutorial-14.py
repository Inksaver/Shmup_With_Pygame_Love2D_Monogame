''' 
Episode 14
https://www.youtube.com/watch?v=Z2K2Yttvr5g
Menu 

# Background music: Frozen Jam by tgfcoder <https://twitter.com/tgfcoder> licensed under CC-BY-3 
'''
# import libraries
import pygame, os, math, random
import player as cls_player
import shared, mob, bullet, circle, shield, explosion, powerup, menu

class data():
	new_bullet_timer:float  = 0
	allow_new_bullet:bool   = True
	background:object		= None
	background_rect:object  = None
	player_img:object       = None
	meteor_img:object       = None
	bullet_img:object       = None
	shield_bar:object	    = None
	player_mini_img:object  = None
	death_explosion         = None
	die_snd_channel:object  = None

mobs:list                 = []
bullets:list              = []
meteor_images             = []
new_bullet_interval:float = 0.2
sounds:dict               = {}
explosion_anim:dict       = {}
explosions:list           = []
powerup_images:dict       = {}
powerups:list             = []
player                    = None

def circle_collides(circle1:object, circle2:object) -> bool:
	''' Check if 2 circles are intersecting (colliding) '''
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
	# if left side of rect1 is beyond rect2 right side
	# OR left side of rect2 is beyond rect1 right side
	if rect1.x > rect2.x + rect2.width or rect2.x > rect1.x + rect1.width:
		return False
	# if top side of rect1 is beyond bottom of rect2
	# OR top side of rect2 is beyond bottom of rect1
	if rect1.y > rect2.y + rect2.height or rect2.y > rect1.y + rect1.height:
		return False
	# code only gets this far if both if statements above fail 
	return True

def draw_lives(surf:object, x:int, y:int, lives:int, img:object) -> None:
	''' draw up to 3 mini ships to represent lives left '''
	for i in range(lives):
		img_rect = img.get_rect()
		img_rect.x = x + 30 * i # draw each ship 30 pixels apart
		img_rect.y = y
		surf.blit(img, img_rect)

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
		for j in range(len(mobs) -1, -1, -1):
			# check collisions of bullet and mob
			destroy = False
			try:
				#destroy = collides(bullets[i].rect , mobs[j].rect)                  # rectangle collider
				destroy = circle_collides(bullets[i].circle, mobs[j].circle) 		# circle collider
			except:
				pass					
			if destroy: 								# collision: so reset mob and delete bullet
				radius = mobs[j].circle.radius
				shared.score += 50 - radius				# higher score for small meteor
				if shared.audio_present:
					sounds["shoot"].stop()				# stop shoot sound
					if radius > 25:
						sounds["expl2"].play()			# play deeper sound
					else:
						sounds["expl1"].play()			# play lighter sound					
				bullets.pop()							# delete bullet
				
				if radius > 25:
					explosion_size = 'lg'
				else:
					explosion_size = 'sm'
				explosions.append(explosion.Explosion(explosion_anim, mobs[j].rect.center, explosion_size))
				
				rnd_value = random.random()  			# generates random decimal between 0 and 1
				if rnd_value > 0.9 or (shared.debug and rnd_value > 0.1): # 1 in 10 chance (9 in 10 in debug mode)
					powerups.append(powerup.Pow(powerup_images, mobs[j].rect.center))	
					
				mobs[j].reset()
				if len(bullets) == 0:
					break	

	''' update remaining bullets '''
	for i in range(len(bullets) - 1, -1, -1):
		if not bullets[i].update(dt):
			bullets.pop()
			
def check_mob_player_collisions(keystate:object, dt:float) -> None:
	''' update Player and all mobs '''
	if player.alive:
		player.update(keystate, dt)
	else: # 0 lives so allow for finish of explosion sound and animation
		if data.death_explosion == None: # Player dead, explosion animation finished
			if not data.die_snd_channel.get_busy(): # is explosion audio completed?
				shared.gamestate = shared.gamestates["menu"]
	for mob in mobs:
		mob.update(dt)
		''' check if any mobs colliding with Player '''
		if not player.hidden:
			#if collides(mob.rect, player.rect):
			if circle_collides(mob.circle, player.circle):
				mob.reset()		
				player.shield -= mob.circle.radius	# reduce player shield
				sounds['shoot'].stop()
				if player.shield <= 0:				# shield gone
					player.lives -= 1				# lose a life
					player.shield = 100	
					if shared.audio_present:
						sounds["die"].play()
					# Alive set to false only when all 3 lives used.
					data.death_explosion = explosion.Explosion(explosion_anim, player.rect.center, 'player')
					if player.lives <= 0:
						player.alive = False
					else:
						player.hide()
				else: # no lives lost but play sound of player hit
					if shared.audio_present:
						sounds["expl3"].play()

def check_powerup_player_collisions(dt:float) -> None:
	''' update and check any powerups colliding with Player '''		
	for i in range(len(powerups) - 1, -1, -1):
		#if collides(mob.rect, shared.player.rect):
		if circle_collides(powerups[i].circle, player.circle):
			if shared.audio_present:
				sounds['shoot'].stop()					
			if powerups[i].key == 'shield':		# powerup is shield, add to its value
				player.shield += random.randrange(10, 30)
				if shared.audio_present:
					sounds['shield'].play()
				if player.shield > 100:
					player.shield = 100
			if powerups[i].key == 'gun':		# powerup is double gun
				player.powerup()
				if shared.audio_present:
					sounds['power'].play()
			powerups.pop()
		else:
			if not powerups[i].update(dt):
				powerups.pop()

def shoot() -> None:
	''' Create a new bullet at player's position '''
	if data.allow_new_bullet and not player.hidden:
		if shared.audio_present:
			sounds["shoot"].stop()
		if player.power == 1:		# single bullet from player top/centre
			bullets.append(bullet.Bullet(data.bullet_img, player, "centre"))
		elif player.power >= 2:		# two bullets, one from each side of the player
			bullets.append(bullet.Bullet(data.bullet_img, player, "left"))
			bullets.append(bullet.Bullet(data.bullet_img, player, "right"))	

		data.new_bullet_timer = 0	# prevent new bullets being made
		data.allow_new_bullet = False

		if shared.audio_present:
			sounds["shoot"].play()
				
def update_explosions(dt:float) -> None:
	''' update explosions '''
	for i in range(len(explosions) - 1, -1, -1): # Go through the explosions in reverse order. .update(dt) returns true/false
		if not explosions[i].update(dt):		 # update. false if end of frames
			explosions.pop()
	if data.death_explosion != None:
		if not data.death_explosion.update(dt):
			data.death_explosion = None

def load_animations() -> None:
	''' load explosion images and scale for second list '''
	img_dir = os.path.join(shared.game_folder, "img")
	explosion_anim['lg'] = [] # large explosion empty list
	explosion_anim['sm'] = [] # small explosion empty list
	explosion_anim['player'] = [] # player explosion
	for i in range(9):
		filename = f'regularExplosion0{i}.png' # clever way of adding sequential filenames
		# could also use: filename = 'regularExplosion0' + str(i) + '.png'
		img = pygame.image.load(os.path.join(img_dir, filename)).convert()
		img.set_colorkey(shared.BLACK)
		img_lg = pygame.transform.scale(img, (75, 75)) # create large explosion list (modify sizes to suit)
		explosion_anim['lg'].append(img_lg)
		img_sm = pygame.transform.scale(img, (32, 32)) # create small explosion list
		explosion_anim['sm'].append(img_sm)
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
	data.player_mini_img = pygame.transform.scale(data.player_img,(25, 19))
	data.player_mini_img.set_colorkey(shared.BLACK)		

	meteor_list = ['meteorBrown_big1.png', 'meteorBrown_big2.png', 'meteorBrown_med1.png', 'meteorBrown_med3.png',
				   'meteorBrown_small1.png', 'meteorBrown_small2.png', 'meteorBrown_tiny1.png']
	for img in meteor_list:
		meteor_images.append(pygame.image.load(os.path.join(img_dir, img)).convert())	

	powerup_images['shield'] = pygame.image.load(os.path.join(img_dir, "shield_gold.png")).convert()
	powerup_images['gun'] = pygame.image.load(os.path.join(img_dir, "bolt_gold.png")).convert()		

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
		#if not shared.debug:
			#pygame.mixer.music.play(loops = -1)			# start background music if loaded
		
def load() -> None:
	global player
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
	load_images()
	load_audio()
	load_animations()

	player = cls_player.Player(data.player_img, 500, 0.5)
	for i in range(len(meteor_images)):					# make 7 mobs, one of each size
		mobs.append(mob.Mob(meteor_images[i]))
	mobs.append(mob.Mob(random.choice(meteor_images)))	# add 1 final random size
	#shared.debug = True no longer required. press x on menu screen
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
			check_powerup_player_collisions(dt)
			data.shield_bar.update(player.shield)
			
		elif shared.gamestate == shared.gamestates["menu"]:			# current gamestate = 'menu'
			menu.update(keystate, key_down, player)					# so update the 'menu' class
			
    
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
		for powerup in powerups:
			powerup.draw()
		draw_lives(shared.screen, shared.WIDTH - 100, 5, player.lives, data.player_mini_img)
		#draw the score
		# draw_text(screen:object, text:str, size:int, x:int, y:int, align:str = 'centre', colour:tuple = WHITE)
		shared.draw_text(shared.screen, str(shared.score), 18, shared.WIDTH / 2, 10)
		data.shield_bar.draw()		# draw shield bar
		
	elif shared.gamestate == shared.gamestates["menu"]:							# current gamestate = 'menu'
		menu.draw("SHMUP!")														# so draw the menu class
	
	if shared.debug:
		shared.draw_text(shared.screen, "Debug mode", 18, 10, shared.HEIGHT - 24, "left", shared.YELLOW)
		
	pygame.display.flip()														# flip display to make it visible
	
def main() -> None:
	''' Run game loop and call other functions from here '''
	shared.WIDTH = 480									# default screen width: alter as required
	shared.HEIGHT = 600									# default screen height: alter as required
	shared.window_title = "Shmup!"						# default window title: change as required	
	load()												# setup window and game assets
	
	''' gameloop '''
	shared.gamestate = shared.gamestates["menu"]		# gamestate starting at 0 ('menu')
	while shared.gamestate < shared.gamestates["quit"]:	# 3 = quit	
		update()										# go through update functions
		draw()											# go through draw functions

	pygame.quit()

main()